namespace sc
{
	using System;
    using System.IO;
    using System.Reflection;
    using System.Reflection.Emit;

    public class Emit
	{
		private TypeBuilder globalScope;
		private MethodBuilder method;
		private ConstructorBuilder cctor;
		private ILGenerator il;
		private ILGenerator il_cctor;
		private readonly string executableName;
		private bool isMain;
		private bool haveMainMethod;

		public Emit(string name)
		{
			executableName = name;
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = Path.GetFileNameWithoutExtension(name);
			string dir = Path.GetDirectoryName(name);

			string moduleName = Path.GetFileName(name);

			Assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save, dir);
			Module = Assembly.DefineDynamicModule(assemblyName + "Module", moduleName);
			haveMainMethod = false;
		}

		public AssemblyBuilder Assembly { get; }

        public ModuleBuilder Module { get; }

        public TypeBuilder InitGlobalScope()
		{
			globalScope = Module.DefineType("Global_Scope", TypeAttributes.Class | TypeAttributes.Public);
			cctor = globalScope.DefineConstructor(MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
			il_cctor = cctor.GetILGenerator();
			il_cctor.BeginScope();

			return globalScope;
		}

		public TypeBuilder DefineStructure(string name)
		{
			return Module.DefineType(name, 
				TypeAttributes.Class | 
				TypeAttributes.AutoLayout | 
				TypeAttributes.Public | 
				TypeAttributes.AnsiClass | 
				TypeAttributes.Serializable,
				typeof(object));
		}

		public TypeBuilder DefineUnion(string name)
		{
			return Module.DefineType(name,
				TypeAttributes.Sealed |
				TypeAttributes.ExplicitLayout |
				TypeAttributes.Public |
				TypeAttributes.AnsiClass |
				TypeAttributes.Serializable,
				typeof(ValueType));
		}

		public void WriteExecutable()
		{
			FinishLastMethod();
			if (!haveMainMethod)
			{
				method = globalScope.DefineMethod("main", MethodAttributes.Static | MethodAttributes.Public, typeof(int), Type.EmptyTypes);
				il = method.GetILGenerator();
				il.Emit(OpCodes.Ldc_I4_0);
				il.Emit(OpCodes.Ret);
				Assembly.SetEntryPoint(method);
			}

			il_cctor.Emit(OpCodes.Ret);
			il_cctor.EndScope();

			globalScope.CreateType();
			Assembly.Save(Path.GetFileName(executableName));
		}

		public void EndScope()
		{
			il.EndScope();
		}

		public void FinishLastMethod()
		{
			if (method == null) return;

			if (isMain)
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}

			il.Emit(OpCodes.Ret);

			EndScope();
		}

		public LocalBuilder LocalVar(Type localVarType)
		{
			LocalBuilder result = il.DeclareLocal(localVarType);
			if (CanInitializeLocation(localVarType))
			{
				// Store the already prepared initializer
				il.Emit(OpCodes.Stloc, result);
			}
			return result;
		}

		private bool CanInitializeLocation(Type Ty)
		{
			if (!Ty.IsValueType)
			{
				if (Ty == typeof(String))
				{
					il.Emit(OpCodes.Ldstr, "");
				}
				else
				{
					ConstructorInfo cons = Ty.GetConstructor(Type.EmptyTypes);
					if (cons != null && !Ty.IsAbstract)
					{
						il.Emit(OpCodes.Newobj, Ty);
					}
					else
					{
						il.Emit(OpCodes.Ldnull);
					}
				}
				return true;
			}

			return false;
		}

		internal FieldInfo AddField(string fieldName, Type type, long arraySize)
		{
			FieldInfo field = globalScope.DefineField(fieldName, type, FieldAttributes.Public | FieldAttributes.Static);
			if (type.IsArray)
			{
				il_cctor.Emit(OpCodes.Ldc_I4, arraySize);
				il_cctor.Emit(OpCodes.Newarr, type.GetElementType());
				il_cctor.Emit(OpCodes.Stsfld, field);
			}
			else if (CanInitializeLocation(type))
			{
				il_cctor.Emit(OpCodes.Stsfld, field);
			}
			return field;
		}
	}
}
typedef int Int32;
typedef int Int32; //expectederror "Identifier Int32 already declared."
struct StructDecl { int test, test2; } StructDecl2;
struct StructDecl { int test, test2; }; //expectederror "Identifier StructDecl already declared.";
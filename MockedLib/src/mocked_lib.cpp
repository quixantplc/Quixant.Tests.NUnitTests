#include <cstring>
#include "../include/native_lib_header.h"

//NATIVE_LIB_API QRESULT nativeFunction1(LIB_HANDLE* handle, char* charPointer) { return mockObj->nativeFunction1(handle, charPointer); }
//NATIVE_LIB_API QRESULT nativeFunction2(LIB_HANDLE handle, STRUCT1 struct1) { return mockObj->nativeFunction2(handle, struct1); }
//NATIVE_LIB_API QRESULT nativeFunction3(LIB_HANDLE handle, PSTRUCT2 struct2) { return mockObj->nativeFunction3(handle, struct2); }
//NATIVE_LIB_API QRESULT nativeFunction4(LIB_HANDLE handle, STRUCT3 struct3) { return mockObj->nativeFunction4(handle, struct3); }
NATIVE_LIB_API QRESULT nativeFunction5(int integerVal) {
    char funcName[16];
    strcpy_s(funcName, sizeof(funcName), __func__);
    funcName[sizeof(funcName) - 1] = '\0'; // Ensure null-termination
    callback(funcName, integerVal);
    return 2 * integerVal;
}

NATIVE_LIB_API void set_native_callback(native_callback* cb) {
    callback = cb; // Assign the pointer directly
}
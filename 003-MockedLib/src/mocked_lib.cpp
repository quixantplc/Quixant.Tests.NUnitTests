#include "mocked_lib_header.h"

static MockedObject* mockObj = nullptr;

MOCK_API void setUpNativeLibMock(MockedObject** mock) {
	mockObj = new NiceMock<MockedObject>();
	*mock = mockObj;
}

MOCK_API void tearDownNativeLibMock(MockedObject** mock) {	
	delete mockObj;
	mockObj = nullptr;
	*mock = nullptr;
}

NATIVE_LIB_API QRESULT nativeFunction1(LIB_HANDLE* handle, char* charPointer) { return mockObj->nativeFunction1(handle, charPointer); }
NATIVE_LIB_API QRESULT nativeFunction2(LIB_HANDLE handle, STRUCT1 struct1) { return mockObj->nativeFunction2(handle, struct1); }
NATIVE_LIB_API QRESULT nativeFunction3(LIB_HANDLE handle, PSTRUCT2 struct2) { return mockObj->nativeFunction3(handle, struct2); }
NATIVE_LIB_API QRESULT nativeFunction4(LIB_HANDLE handle, STRUCT3 struct3) { return mockObj->nativeFunction4(handle, struct3); }
NATIVE_LIB_API QRESULT nativeFunction5(int integerVal) { return mockObj->nativeFunction5(integerVal); }


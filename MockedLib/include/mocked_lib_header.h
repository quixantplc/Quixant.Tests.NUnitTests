#pragma once

#include <gmock/gmock.h>
#include "native_lib_header.h"

#ifdef _WIN32
#ifdef __cplusplus
#define MOCK_API extern "C" __declspec(dllexport)
#else
#define MOCK_API __declspec(dllexport)
#endif
#else
#ifdef __cplusplus
#define MOCK_API extern "C"
#else
#define MOCK_API
#endif
#endif

using ::testing::NiceMock;

class MockedObject {
public:
	MockedObject() = default;
	virtual ~MockedObject() = default;

	MOCK_METHOD(QRESULT, nativeFunction1, (LIB_HANDLE* handle, char* charPointer), ());
	MOCK_METHOD(QRESULT, nativeFunction2, (LIB_HANDLE handle, STRUCT1 struct1), ());
	MOCK_METHOD(QRESULT, nativeFunction3, (LIB_HANDLE handle, PSTRUCT2 struct2), ());
	MOCK_METHOD(QRESULT, nativeFunction4, (LIB_HANDLE handle, STRUCT3 struct3), ());
	MOCK_METHOD(QRESULT, nativeFunction5, (int integerVal), ());
};

MOCK_API void setUpNativeLibMock(MockedObject** mock);
MOCK_API void tearDownNativeLibMock(MockedObject** mock);
#pragma once

#ifdef _WIN32
#ifndef NATIVE_LIB_API
#ifdef __cplusplus
#define NATIVE_LIB_API extern "C" __declspec(dllexport)
#else
#define NATIVE_LIB_API __declspec(dllexport)
#endif
#endif
#else
#ifndef NATIVE_LIB_API
#ifdef __cplusplus
#define NATIVE_LIB_API extern "C"
#else
#define NATIVE_LIB_API 
#endif
#endif
#endif

#ifndef QRESULT
#define QRESULT unsigned int
#endif

#ifndef Q_SUCCESS
#define Q_SUCCESS 0
#endif

#pragma pack(push, 8)
// structs go here
typedef void* LIB_HANDLE;

typedef struct _STRUCT1
{
	unsigned int	BaudRate;
	unsigned int	Databits;
	unsigned int	Parity;
	unsigned int	StopBits;
	unsigned char	EnableTimestamps;
} STRUCT1, * PSTRUCT1;

typedef struct _STRUCT2
{
	unsigned int	BaudRate;
	unsigned int	Databits;
	unsigned int	Parity;
	unsigned int	StopBits;
	unsigned char	EnableTimestamps;
} STRUCT2, * PSTRUCT2;

typedef struct _STRUCT3
{
	long long IntervalTimeout;
	long long ReadTimeout;
	long long WriteTimeout;
} STRUCT3, * PSTRUCT3;

#pragma pack(pop)

NATIVE_LIB_API QRESULT nativeFunction1(LIB_HANDLE* handle, char* charPointer);
NATIVE_LIB_API QRESULT nativeFunction2(LIB_HANDLE handle, STRUCT1 struct1);
NATIVE_LIB_API QRESULT nativeFunction3(LIB_HANDLE handle, PSTRUCT2 struct2);
NATIVE_LIB_API QRESULT nativeFunction4(LIB_HANDLE handle, STRUCT3 struct3);
NATIVE_LIB_API QRESULT nativeFunction5(int integerVal);

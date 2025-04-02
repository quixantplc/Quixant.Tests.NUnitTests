#include <gtest/gtest.h>
#include "mocked_lib_header.h"

#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include <assert.h>
#include <chrono>
#include <iostream>
#include <thread>
#include <vector>


// Provided by the AppHost NuGet package and installed as an SDK pack
#include <nethost.h>

// Header files copied from https://github.com/dotnet/core-setup
#include <coreclr_delegates.h>
#include <hostfxr.h>

#ifdef _WIN32
#include <Windows.h>

#define STR(s) L ## s
#define CH(c) L ## c
#define DIR_SEPARATOR L'\\'

#define string_compare wcscmp

#else
#include <dlfcn.h>
#include <limits.h>

#define STR(s) s
#define CH(c) c
#define DIR_SEPARATOR '/'
#define MAX_PATH PATH_MAX

#define string_compare strcmp

#endif

using ::testing::Test;
using ::testing::_;

using string_t = std::basic_string<char_t>;

// Globals to hold hostfxr exports
hostfxr_initialize_for_dotnet_command_line_fn init_for_cmd_line_fptr;
hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
hostfxr_get_runtime_delegate_fn get_delegate_fptr;
hostfxr_run_app_fn run_app_fptr;
hostfxr_close_fn close_fptr;

load_assembly_and_get_function_pointer_fn get_fn_ptr;

// Forward declarations
bool load_hostfxr(const char_t* app);
load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t* assembly);

class MyTestEnvironment : public ::testing::Environment {
public:
	MyTestEnvironment(const string_t& program_name) : program_name_(program_name) {}

	const string_t& GetProgramName() const {
		return program_name_;
	}

private:
	string_t program_name_;
};

MyTestEnvironment* test_env;

class TestBase : public ::testing::Test
{
public:
	static void SetUpTestSuite()
	{
		// Setup .net hosting
		// Get the current executable's directory
		// This sample assumes the managed assembly to load and its runtime configuration file are next to the host
		char_t host_path[MAX_PATH];
#if _WIN32
		auto size = ::GetFullPathNameW(test_env->GetProgramName().c_str(), sizeof(host_path) / sizeof(char_t), host_path, nullptr);
		assert(size != 0);
#else
		auto resolved = realpath(test_env->GetProgramName(), host_path);
		assert(resolved != nullptr);
#endif

		root_path = host_path;
		auto pos = root_path.find_last_of(DIR_SEPARATOR);
		assert(pos != string_t::npos);
		root_path = root_path.substr(0, pos + 1);

		//
		// STEP 1: Load HostFxr and get exported hosting functions
		//
		if (!load_hostfxr(nullptr))
		{
			assert(false && "Failure: load_hostfxr()");
			exit(-1);
		}

		//
		// STEP 2: Initialize and start the .NET Core runtime
		//
		const string_t config_path = root_path + STR("DotNetWrapper.runtimeconfig.json");
		load_assembly_and_get_function_pointer = get_dotnet_load_assembly(config_path.c_str());
		assert(load_assembly_and_get_function_pointer != nullptr && "Failure: get_dotnet_load_assembly()");

		//// Get delegate for creating .NET objects
		//get_delegate_fn = (hostfxr_get_runtime_delegate_fn)dlsym(hostfxr_lib, "hostfxr_get_runtime_delegate");
		////load_assembly_and_get_function_pointer_fn get_fn_ptr;
		//rc = get_delegate_fn(handle, hdt_load_assembly_and_get_function_pointer, (void**)&get_fn_ptr);
		//if (rc != 0 || get_fn_ptr == nullptr)
		//{
		//    std::cerr << "Failed to get function pointer" << std::endl;
		//    return -1;
		//}
	}

	static void TearDownTestSuite()
	{

	}

	void SetUp() override
	{
		setUpNativeLibMock(&mock);
	}

	void TearDown() override
	{
		tearDownNativeLibMock(&mock);
	}
protected:
	MockedObject* mock = nullptr;
	static load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer;
	static string_t root_path;
};

load_assembly_and_get_function_pointer_fn TestBase::load_assembly_and_get_function_pointer = nullptr;
string_t TestBase::root_path{};


TEST_F(TestBase, testingNativeFunction5)
{
	EXPECT_CALL(*mock, nativeFunction5(_)).WillOnce(::testing::Return(2));

	const string_t dotnetlib_path = root_path + STR("DotNetWrapper.dll");
	const char_t* dotnet_type = STR("NativeLibWrapper, DotNetWrapper");
	int rc = 0;

	typedef unsigned int (CORECLR_DELEGATE_CALLTYPE* nativeFunction5_fn)(int integerVal);
	nativeFunction5_fn custom = nullptr;

	// Custom delegate type
	rc = load_assembly_and_get_function_pointer(
		dotnetlib_path.c_str(),
		dotnet_type,
		STR("nativeFunction5") /*method_name*/,
		STR("NativeLibWrapper+nativeFunction5Delegate, DotNetWrapper") /*delegate_type_name*/,
		nullptr,
		(void**)&custom);
	assert(rc == 0 && custom != nullptr && "Failure: load_assembly_and_get_function_pointer()");
	auto res = custom(10);

	EXPECT_EQ(res, 2);
}

TEST_F(TestBase, testingNativeFunction5WithFactory)
{
	EXPECT_CALL(*mock, nativeFunction5(_)).WillOnce(::testing::Return(2));

	const string_t dotnetlib_path = root_path + STR("FactoryHelper.dll");
	const char_t* dotnet_type = STR("FactoryHelper.FactoryHelper, FactoryHelper");
	int rc = 0;

	typedef void* (CORECLR_DELEGATE_CALLTYPE* Factory_fn)(char* assemblyName, char* typeName, char* methodName,
		int ctorParams, ...);
	Factory_fn custom = nullptr;

	// Custom delegate type
	rc = load_assembly_and_get_function_pointer(
		dotnetlib_path.c_str(),
		dotnet_type,
		STR("Factory") /*method_name*/,
		STR("FactoryHelper.FactoryHelper+FactoryDelegate, FactoryHelper") /*delegate_type_name*/,
		nullptr,
		(void**)&custom);
	assert(rc == 0 && custom != nullptr && "Failure: load_assembly_and_get_function_pointer()");
	char assemblyName[20];
	strcpy_s(assemblyName, sizeof(assemblyName), "DotNetWrapper");
	assemblyName[sizeof(assemblyName) - 1] = '\0'; // Ensure null-termination
	char typeName[20];
	strcpy_s(typeName, sizeof(typeName), "NonStaticWrapper");
	typeName[sizeof(typeName) - 1] = '\0'; // Ensure null-termination
	char methodName[20];
	strcpy_s(methodName, sizeof(methodName), "Function5");
	methodName[sizeof(methodName) - 1] = '\0'; // Ensure null-termination
	int param = 10;
	auto res = custom(assemblyName, typeName, methodName, 0, &param);
	EXPECT_EQ(*((int*)res), 2);
}

// ndr: calling member functions of non-static class (evaluate reflection)
TEST_F(TestBase, callingNonStaticClassAndMethod)
{
	// TODO


}

#if _WIN32
int __cdecl wmain(int argc, wchar_t* argv[])
#else
int main(int argc, char* argv[])
#endif
{
	::testing::InitGoogleTest(&argc, argv);

	test_env = new MyTestEnvironment(argv[0]);
	::testing::AddGlobalTestEnvironment(test_env);

	return RUN_ALL_TESTS();
}

/********************************************************************************************
* Function used to load and activate .NET Core
********************************************************************************************/

// Forward declarations
void* load_library(const char_t*);
void* get_export(void*, const char*);

#ifdef _WIN32
void* load_library(const char_t* path)
{
	HMODULE h = ::LoadLibraryW(path);
	assert(h != nullptr);
	return (void*)h;
}
void* get_export(void* h, const char* name)
{
	void* f = ::GetProcAddress((HMODULE)h, name);
	assert(f != nullptr);
	return f;
}
#else
void* load_library(const char_t* path)
{
	void* h = dlopen(path, RTLD_LAZY | RTLD_LOCAL);
	assert(h != nullptr);
	return h;
}
void* get_export(void* h, const char* name)
{
	void* f = dlsym(h, name);
	assert(f != nullptr);
	return f;
}
#endif

// <SnippetLoadHostFxr>
// Using the nethost library, discover the location of hostfxr and get exports
bool load_hostfxr(const char_t* assembly_path)
{
	get_hostfxr_parameters params{ sizeof(get_hostfxr_parameters), assembly_path, nullptr };
	// Pre-allocate a large buffer for the path to hostfxr
	char_t buffer[MAX_PATH];
	size_t buffer_size = sizeof(buffer) / sizeof(char_t);
	int rc = get_hostfxr_path(buffer, &buffer_size, &params);
	if (rc != 0)
		return false;

	// Load hostfxr and get desired exports
	// NOTE: The .NET Runtime does not support unloading any of its native libraries. Running
	// dlclose/FreeLibrary on any .NET libraries produces undefined behavior.
	void* lib = load_library(buffer);
	init_for_cmd_line_fptr = (hostfxr_initialize_for_dotnet_command_line_fn)get_export(lib, "hostfxr_initialize_for_dotnet_command_line");
	init_for_config_fptr = (hostfxr_initialize_for_runtime_config_fn)get_export(lib, "hostfxr_initialize_for_runtime_config");
	get_delegate_fptr = (hostfxr_get_runtime_delegate_fn)get_export(lib, "hostfxr_get_runtime_delegate");
	run_app_fptr = (hostfxr_run_app_fn)get_export(lib, "hostfxr_run_app");
	close_fptr = (hostfxr_close_fn)get_export(lib, "hostfxr_close");

	return (init_for_config_fptr && get_delegate_fptr && close_fptr);
}
// </SnippetLoadHostFxr>

// <SnippetInitialize>
// Load and initialize .NET Core and get desired function pointer for scenario
load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t* config_path)
{
	// Load .NET Core
	void* load_assembly_and_get_function_pointer = nullptr;
	hostfxr_handle cxt = nullptr;
	int rc = init_for_config_fptr(config_path, nullptr, &cxt);
	if (rc != 0 || cxt == nullptr)
	{
		std::cerr << "Init failed: " << std::hex << std::showbase << rc << std::endl;
		close_fptr(cxt);
		return nullptr;
	}

	// Get the load assembly function pointer
	rc = get_delegate_fptr(
		cxt,
		hdt_load_assembly_and_get_function_pointer,
		&load_assembly_and_get_function_pointer);
	if (rc != 0 || load_assembly_and_get_function_pointer == nullptr)
		std::cerr << "Get delegate failed: " << std::hex << std::showbase << rc << std::endl;

	close_fptr(cxt);
	return (load_assembly_and_get_function_pointer_fn)load_assembly_and_get_function_pointer;
}
// </SnippetInitialize>

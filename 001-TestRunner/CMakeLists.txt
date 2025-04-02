cmake_minimum_required( VERSION 3.15 )


# Path to .NET SDK
set(DOTNET_ROOT "C:/Program Files/dotnet")

#include_directories(${DOTNET_ROOT}/packs/Microsoft.NETCore.App.Host.win-x64/9.0.0/runtimes/win-x64/native/include)
#link_directories(${DOTNET_ROOT}/host/fxr/9.0.0)

add_executable(test_runner ${CMAKE_CURRENT_LIST_DIR}/src/TestRunner.cpp)

find_library(hostfxr NAMES hostfxr.dll HINTS ${DOTNET_ROOT}/host/fxr/9.0.0)
target_include_directories(test_runner PRIVATE ${CMAKE_CURRENT_LIST_DIR}/include ${DOTNET_ROOT}/packs/Microsoft.NETCore.App.Host.win-x64/9.0.2/runtimes/win-x64/native)
find_library(nethost NAMES nethost.lib HINTS ${DOTNET_ROOT}/packs/Microsoft.NETCore.App.Host.win-x64/9.0.2/runtimes/win-x64/native)
target_link_libraries(test_runner PRIVATE libmocked $<$<PLATFORM_ID:Linux>:dl> ${nethost} gtest gmock)

#add_custom_target(copy_dlls ALL
#    COMMAND ${CMAKE_COMMAND} -E copy_if_different ${hostfxr} ${CMAKE_RUNTIME_OUTPUT_DIRECTORY}
#)

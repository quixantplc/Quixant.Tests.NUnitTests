cmake_minimum_required( VERSION 3.15 )

add_library(libmocked SHARED ${CMAKE_CURRENT_SOURCE_DIR}/src/mocked_lib.cpp)

MESSAGE("DEBUG " ${CMAKE_CURRENT_SOURCE_DIR})
target_include_directories(libmocked PUBLIC ${CMAKE_CURRENT_SOURCE_DIR}/include)

if (UNIX)
	#set (LINK_ADDS "stdc++" "m" "dl" "pthread" "rt" "sensors")
	set (LINK_ADDS "stdc++" "m" "dl" "pthread" "rt")
else()
	set (LINK_ADDS "winmm" "setupapi")
endif()

target_link_libraries(libmocked gtest gmock ${LINK_ADDS})
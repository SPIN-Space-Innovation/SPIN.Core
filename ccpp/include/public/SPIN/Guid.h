/** \if false
 * MIT License
 * 
 * Copyright (c) 2023 SPIN - Space Innovation
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * \endif
 */

#pragma once

#if !defined(__cplusplus)
    #include <stdbool.h>
    #include <stddef.h>
    #include <stdint.h>
#else
    #include <cstddef>
    #include <cstdint>
#endif

#if defined(__cplusplus)
extern "C"
{
#endif

struct SPIN_Core_Guid_t {
    uint32_t Data1;
    uint16_t Data2;
    uint16_t Data3;
    uint64_t Data4;
};

bool SPIN_Core_Guid_EmptyGuid(struct SPIN_Core_Guid_t* guid);

bool SPIN_Core_Guid_NewGuid(struct SPIN_Core_Guid_t* guid);

bool SPIN_Core_Guid_Equals(struct SPIN_Core_Guid_t* guidLeft, struct SPIN_Core_Guid_t* guidRight, bool* result);

int32_t SPIN_Core_Guid_ToString(struct SPIN_Core_Guid_t* guid, const char* fmt, char* str, size_t strSize);

#if defined(__cplusplus)
}
#endif

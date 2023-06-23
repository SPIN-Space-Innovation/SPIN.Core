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

#include "SPIN/Core/Guid.h"

#include <ctype.h>
#include <stdbool.h>
#include <stddef.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

static const char* _formatters[26] = {
    "",
    "{%02X%02X%02X%02X-%02X%02X-%02X%02X-%02X%02X-%02X%02X%02X%02X%02X%02X}",
    "",
    "%02X%02X%02X%02X-%02X%02X-%02X%02X-%02X%02X-%02X%02X%02X%02X%02X%02X",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X",
    "",
    "(%02X%02X%02X%02X-%02X%02X-%02X%02X-%02X%02X-%02X%02X%02X%02X%02X%02X)",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "{0x%02X%02X%02X%02X,0x%02X%02X,0x%02X%02X,{0x%02X,0x%02X,0x%02X,0x%02X,0x%02X,0x%02X,0x%02X,0x%02X}}",
    "",
    ""
};

static int32_t _formatterSizes[26] = {
    INT32_C(-1),
    INT32_C(38),
    INT32_C(-1),
    INT32_C(36),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(32),
    INT32_C(-1),
    INT32_C(38),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(-1),
    INT32_C(68),
    INT32_C(-1),
    INT32_C(-1)
};

static const char* _defaultFmt = "D";

int32_t SPIN_Core_Guid_ToString(struct SPIN_Core_Guid_t* guid, const char* fmt, char* str, size_t strSize)
{
    bool guidIsNull = (guid == (struct SPIN_Core_Guid_t*)NULL);
    if (guidIsNull)
    {
        return -1;
    }

    bool fmtIsNull = ((fmt == (const char*)NULL) || (strcmp(fmt, "") == 0));
    if (fmtIsNull)
    {
        fmt = _defaultFmt;
    }

    size_t fmtLenght = strlen(fmt);
    bool fmtLenghtIsInvalid = (fmtLenght != 1);
    if (fmtLenghtIsInvalid)
    {
        return -1;
    }

    size_t index = (toupper(*fmt) - 'A') % 26;

    bool requestingSize = (str == (char*)NULL);
    if (requestingSize)
    {
        return _formatterSizes[index];
    }

    int32_t returnValue = snprintf(str,
        strSize,
        _formatters[index],
        ((uint8_t*)guid)[0],
        ((uint8_t*)guid)[1],
        ((uint8_t*)guid)[2],
        ((uint8_t*)guid)[3],
        ((uint8_t*)guid)[4],
        ((uint8_t*)guid)[5],
        ((uint8_t*)guid)[6],
        ((uint8_t*)guid)[7],
        ((uint8_t*)guid)[8],
        ((uint8_t*)guid)[9],
        ((uint8_t*)guid)[10],
        ((uint8_t*)guid)[11],
        ((uint8_t*)guid)[12],
        ((uint8_t*)guid)[13],
        ((uint8_t*)guid)[14],
        ((uint8_t*)guid)[15]);

    return returnValue;
}

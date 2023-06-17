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

#include "SPIN/Guid.h"

#include <stdbool.h>
#include <stddef.h>
#include <string.h>

#define EQUAL_COMPARISON (0)

bool SPIN_Core_Guid_Equals(struct SPIN_Core_Guid_t* guidLeft, struct SPIN_Core_Guid_t* guidRight, bool* result)
{
    bool guidLeftIsNull = (guidLeft == (struct SPIN_Core_Guid_t*)NULL);
    bool guidRightIsNull = (guidRight == (struct SPIN_Core_Guid_t*)NULL);
    bool resultIsNull = (result == (bool*)NULL);

    bool argumentsAreInvalid = (guidLeftIsNull || guidRightIsNull || resultIsNull);
    if (argumentsAreInvalid)
    {
        return false;
    }

    int comparisonResult = memcmp((const void*)guidLeft, (const void*)guidRight, sizeof(*guidLeft));
    bool guidsAreEqual = (comparisonResult == EQUAL_COMPARISON);

    return guidsAreEqual;
}

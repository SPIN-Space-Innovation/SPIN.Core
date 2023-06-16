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
#include <stdint.h>
#include <stdlib.h>

bool SPIN_Guid_NewGuid(struct SPIN_Guid_t* guid)
{
    bool guidIsNull = (guid == (struct SPIN_Guid_t*)NULL);
    if (guidIsNull)
    {
        return false;
    }

    for (int i = 0; i < sizeof(*guid); i++)
    {
        ((uint8_t*)guid)[i] = (uint8_t)rand();
    }

    ((uint8_t*)guid)[6] &= 0x0F;
    ((uint8_t*)guid)[6] |= 0x40;


    ((uint8_t*)guid)[8] &= 0x3F;
    ((uint8_t*)guid)[8] |= 0x80;

    return true;
}

#region License
// Copyright 2012 Jason Walker
// ungood@onetrue.name
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and 
// limitations under the License.
#endregion

using System;
using System.Threading.Tasks;
using NSubstitute;

namespace Niles.Test
{
    public static class NSubstituteExtensions
    {
        public static void ReturnsAsync<T>(this object target, T objectToReturn)
        {
            target.Returns(info => {
                var task = new Task<T>(() => objectToReturn);
                task.Start();
                return task;
            });
        }

        public static void ReturnsForAnyArgsAsync<T>(this object target, T objectToReturn)
        {
            target.ReturnsForAnyArgs(info => {
                var task = new Task<T>(() => objectToReturn);
                task.Start();
                return task;
            });
        }

        public static void Throws<T>(this T target, Exception ex)
        {
            target.Returns(info => { throw ex; });
        }

        public static void ThrowsForAnyArgs<T>(this T target, Exception ex)
        {
            target.ReturnsForAnyArgs(info => { throw ex; });
        }
    }
}

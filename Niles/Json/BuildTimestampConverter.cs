#region License
// Copyright 2011 Jason Walker
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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Niles.Json
{
    public class BuildTimestampConverter : DateTimeConverterBase
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var delta = ((DateTime) value) - Epoch;
                if (delta.TotalMilliseconds < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "Unix epoch starts January 1st, 1970");
                }
                ticks = (long) delta.TotalSeconds;
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(ticks);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(
                    String.Format("Unexpected token parsing date. Expected Integer, got {0}.",
                        reader.TokenType));
            }

            var ms = (long) reader.Value;
            return Epoch.AddMilliseconds(ms);
        }
    }
}

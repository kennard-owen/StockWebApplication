﻿using Swifter.Json;
using System;
using System.Collections.Generic;
using System.Text;
using PF.Utils.Extensions;
using Swifter.RW;

namespace PF.Utils.Json
{

    public static class SwifterJson
    {
        public static string SwifterToJson<T>(this T obj)
        {
            return JsonFormatter.SerializeObject(obj);
        }

        public static string SwifterToJson<T>(this T obj, string dateTimeFormat, JsonFormatterOptions options = JsonFormatterOptions.Default)
        {
            var format = new JsonFormatter(options);
            format.SetDateTimeFormat(dateTimeFormat.IsEmpty() ? DateTimeExt.DateTimeFormat : dateTimeFormat);
            return format.Serialize(obj);
        }

        public static T SwifterToObj<T>(this string str)
        {
            return JsonFormatter.DeserializeObject<T>(str);
        }
    }
}
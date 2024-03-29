﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIt.Core.Common.DTO {
    public class Result {
        public bool Succeeded { get; private set; }

        public string[] Errors { get; private set; }


        private Result(bool succeeded, IEnumerable<string> errors) {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }


        public static Result Success() {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IEnumerable<string> errors) {
            return new Result(false, errors);
        }

        public static Result Failure(params string[] error) {
            return new Result(false, error);
        }
    }
}

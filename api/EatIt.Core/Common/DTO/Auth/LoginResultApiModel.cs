﻿
namespace EatIt.Core.Common.DTO.Auth {
    public class LoginResultApiModel {
        public Result Result { get; private set; }

        public string? Token { get; private set; } = null;

        public DateTime? Expiration { get; private set; } = null;

        public string? Username { get; private set; } = null;

        public string? Email { get; private set; } = null;


        public LoginResultApiModel() {
            this.Result = Result.Failure("Login failed");
        }

        public LoginResultApiModel(Result res) {
            this.Result = res;
        }

        public LoginResultApiModel(Result res, string token, DateTime expiration, string Username, string Email) {
            this.Result = res;
            this.Token = token;
            this.Expiration = expiration;
            this.Username = Username;
            this.Email = Email;
        }
    }
}

namespace AuthService.API.Helpers
{
    namespace AuthService.API.Helpers
    {
        public static class AuthMessages
        {
            public const string LoginSuccess = "Login successful.";
            public const string LoginFailed = "Incorrect email or password.";
            public const string EmailNotVerified = "Email is not verified. Please check your inbox.";
            public const string AccountLocked = "Your account is locked. Please contact administrator.";
            public const string AccountNotFound = "Account does not exist.";
            public const string PasswordChanged = "Password changed successfully.";
            public const string ResetEmailSent = "Password reset email has been sent.";
            public const string PasswordResetSuccess = "Your password has been reset successfully.";
            public const string InvalidOrExpiredToken = "Invalid or expired token.";
            public const string EmailAlreadyExists = "Email already exists.";
            public const string RegisterSuccess = "Registration successful. Please verify your email.";
            public const string LogoutSuccess = "Logout successful.";
            public const string SetPasswordSuccess = "Password has been set successfully. You can now log in.";
            public const string RoleInvalid = "Invalid role.";
            public const string LocationInvalid = "Invalid location.";
            public const string CreateSuccess = "Account has been created. Please check your email to set a password.";
            public const string UnauthorizedLocationCreation = "Admins can only create users within their assigned location.";
            public const string UnsupportedProfileInfo = "Unsupported profile information type.";
            public const string EmailNotFound = "Email does not exist.";


         
            public const string RoleNotFound = "Role 'User' does not exist.";
            public const string InvalidLocation = "Invalid location. Please select a supported one.";
            public const string AdminAlreadyExists = "Email already exists.";
           
            public const string AdminCreatedAndVerified = "Admin account has been created and verified.";
            public const string AdminCreatedNeedVerify = "Admin account has been created. Please verify the email.";

           
            
            public const string RoleAlreadyExists = "Email already exists.";
            public const string SystemAccountCreated = "The {0} account has been created. Please check your email to set the password.";
            public const string InvalidToken = "Invalid or non-existent token.";
            public const string TokenExpired = "Token has expired.";
  

        }
    }

}

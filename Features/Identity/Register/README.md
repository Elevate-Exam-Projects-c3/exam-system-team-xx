# Register Feature

Handles public user registration. A new account is created with the **Student** role in a `Pending` status, and an email-verification OTP is sent to the user.

## Endpoint

`POST /api/auth/register`

### Request body

```json
{
  "fullName": "Jane Doe",
  "email": "jane@example.com",
  "password": "P@ssw0rd!"
}
```

| Field      | Rules                                  |
|------------|----------------------------------------|
| `fullName` | Required, 2–100 characters             |
| `email`    | Required, valid email address          |
| `password` | Required (plus ASP.NET Identity rules) |

### Responses

| Status          | Meaning                                                              |
|-----------------|----------------------------------------------------------------------|
| `201 Created`   | User created and verification OTP sent                               |
| `400 Bad Request` | Validation failed, email already exists, or Identity rejected the password |
| Other errors    | Derived from the failed `Result` (see `Features/Shared/ResultPattern`) |

## Flow

1. `RegisterController` receives the `RegisterRequest` and sends a `RegisterOrchestrator` via MediatR.
2. `RegisterOrchestratorHandler` coordinates the process:
   1. Sends `UserExistsQuery` – fails with a validation error if the email is already registered.
   2. Sends `RegisterCommand` to create the account.
3. `RegisterCommandHandler`:
   1. Maps the request to `ApplicationUser` (`UserName = Email`, `AccountStatus = Pending`, `EmailConfirmed = false`).
   2. Creates the user through `UserManager`.
   3. Verifies the `Student` role exists and assigns it. If role assignment fails, the created user is deleted (rollback).
   4. Generates and sends the email-verification OTP through `IOtpService`.

## Folder structure

| Folder          | Contents                                                        |
|-----------------|-----------------------------------------------------------------|
| `Controllers`   | `RegisterController` – HTTP endpoint                            |
| `Requests`      | `RegisterRequest` – client payload                              |
| `Orchestrators` | `RegisterOrchestrator` – request that triggers the full flow    |
| `Commands`      | `RegisterCommand` – creates the user                            |
| `Queries`       | `UserExistsQuery` – checks whether an email is registered       |
| `Handlers`      | Handlers for the orchestrator, command and query                |
| `Validators`    | `RegisterValidator` – FluentValidation rules for the command    |
| `HashingPass`   | `BCryptPasswordHasher` – password hashing                       |

## Dependencies

- MediatR (orchestrator/command/query dispatch)
- FluentValidation (input validation via the validation pipeline behavior)
- ASP.NET Core Identity (`UserManager`, `RoleManager`)
- Mapster (`IMapper`)
- `IOtpService` (`Features/Shared/Services`) for OTP generation and email delivery
- Result pattern (`Features/Shared/ResultPattern`)

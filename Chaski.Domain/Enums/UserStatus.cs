namespace Chaski.Domain.Enums;

public enum UserStatus
{
    PendingEmailConfirmation, // Añade este nuevo estado
    Active,
    Inactive,
    Suspended,
    Banned // Podrías añadir otros estados según necesites
}
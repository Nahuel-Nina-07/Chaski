namespace Chaski.Application.Common;

public static class HttpStatusMessages
{
    private static readonly Dictionary<int, string> _messages = new()
    {
        { 200, "La solicitud se completó con éxito." },
        { 201, "El recurso fue creado correctamente." },
        { 202, "La solicitud ha sido aceptada para procesamiento." },
        { 400, "Solicitud incorrecta." },
        { 401, "No autorizado." },
        { 403, "Prohibido." },
        { 404, "Recurso no encontrado." },
        { 409, "Conflicto detectado." },
        { 500, "Error interno del servidor." },
        { 503, "Servicio no disponible." },
        { 504, "Tiempo de espera agotado." }
    };

    public static string GetMessage(int statusCode) =>
        _messages.TryGetValue(statusCode, out var message) ? message : "Código de estado desconocido.";
}
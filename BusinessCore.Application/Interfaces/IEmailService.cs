using System.Threading.Tasks;

namespace BusinessCore.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de correo electrónico
    /// Define las operaciones para el envío de correos
    /// </summary>
    public interface IEmailService
    {
        // Envío básico
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml);
        Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string fileName);

        // Correos específicos
        Task<bool> SendWelcomeEmailAsync(string to, string name);
        Task<bool> SendOrderConfirmationEmailAsync(string to, string orderNumber);
        Task<bool> SendOrderShippedEmailAsync(string to, string orderNumber, string trackingNumber);
        Task<bool> SendOrderDeliveredEmailAsync(string to, string orderNumber);
        Task<bool> SendPasswordResetEmailAsync(string to, string resetToken);
        Task<bool> SendEmailConfirmationEmailAsync(string to, string confirmationToken);
        Task<bool> SendInvoiceEmailAsync(string to, string invoiceNumber, string pdfUrl);
        Task<bool> SendPaymentConfirmationEmailAsync(string to, string orderNumber, decimal amount);
        Task<bool> SendLowStockAlertEmailAsync(string to, string productName, int stock);
        Task<bool> SendPromotionalEmailAsync(string to, string subject, string content);
    }
}
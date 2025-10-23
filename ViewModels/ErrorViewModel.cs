// ============================================================================
// === File: ErrorViewModel.cs
// === Description: Provides a simple view model for handling and displaying 
// ===              error information in the user interface, typically used 
// ===              by the Error view to show request-related diagnostic data.
// ============================================================================

namespace Klemmbausteine.ViewModels
{
    public class ErrorViewModel
    {
        // --- Unique identifier of the request that caused the error
        public string? RequestId { get; set; }

        // --- Indicates whether the RequestId should be displayed
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

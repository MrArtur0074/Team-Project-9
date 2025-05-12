public class ExportResult
{
	public bool IsSuccess { get; }
	public string FilePath { get; }
	public string ErrorMessage { get; }

	private ExportResult(bool isSuccess, string filePath, string errorMessage) {
		IsSuccess = isSuccess;
		FilePath = filePath;
		ErrorMessage = errorMessage;
	}

	public static ExportResult Success(string filePath) => new ExportResult(true, filePath, null);
	public static ExportResult Failure(string errorMessage) => new ExportResult(false, null, errorMessage);
}
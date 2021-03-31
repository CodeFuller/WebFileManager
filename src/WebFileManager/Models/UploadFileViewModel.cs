using Microsoft.AspNetCore.Http;

namespace WebFileManager.Models
{
	public class UploadFileViewModel
	{
		public IFormFile File { get; set; }
	}
}

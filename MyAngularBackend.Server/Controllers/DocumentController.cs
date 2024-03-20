using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TXTextControl;
using TXTextControl.Web.MVC.DocumentViewer;
using TXTextControl.Web.MVC.DocumentViewer.Models;

namespace MyAngularBackend.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DocumentController : ControllerBase
	{

		private readonly ILogger<DocumentController> _logger;

		public DocumentController(ILogger<DocumentController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[Route("Load")]
		public DocumentData Load()
		{
			// open file as byte and convert to base64
			byte[] fileBytes = System.IO.File.ReadAllBytes("App_Data/document.tx");
			string file = Convert.ToBase64String(fileBytes);
			return new DocumentData { Name = "document.pdf", Document = file };
		}

		[HttpPost]
		[Route("Sign")]
		public string Sign([FromBody] SignatureData signatureData)
		{
			byte[] bPDF;

			// create temporary ServerTextControl
			using (ServerTextControl tx = new ServerTextControl())
			{
				tx.Create();

				// load the document
				tx.Load(Convert.FromBase64String(signatureData.SignedDocument.Document),
					BinaryStreamType.InternalUnicodeFormat);

				// create a certificate
				X509Certificate2 cert = new X509Certificate2("App_Data/textcontrol_self.pfx", "123");

				// create a list of digital signatures
				var signatureFields = new List<DigitalSignature>();

				// create a digital signature for each signature box
				foreach (SignatureBox box in signatureData.SignatureBoxes)
				{
					signatureFields.Add(new DigitalSignature(cert, null, box.Name));
				}

				// create save settings
				SaveSettings saveSettings = new SaveSettings()
				{
					CreatorApplication = "Your Application",
					SignatureFields = signatureFields.ToArray()
				};

				// save the document as PDF
				tx.Save(out bPDF, BinaryStreamType.AdobePDFA, saveSettings);
			}

			// return as Base64 encoded string
			return Convert.ToBase64String(bPDF);
		}
	}
}

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAbridged.Operations
{
	public class FileUploadOperation : IOperationFilter
	{
		public void Apply(Operation operation, OperationFilterContext context)
		{
			if(operation.OperationId.ToLower() == "compresspng")
			{
				operation.Parameters.Clear();
				operation.Parameters.Add(new NonBodyParameter
				{
					Name = "pngImgFile",
					In = "formData",
					Description = "Upload File",
					Required = true,
					Type = "file"
				});

				operation.Consumes.Add("multipart/form-data");
			}

			if(operation.OperationId.ToLower() == "convertimagetowebp")
			{
				operation.Parameters.Clear();
				operation.Parameters.Add(new NonBodyParameter
				{
					Name = "imageFiles",
					In = "formData",
					Description = "Upload File",
					Required = true,
					Type = "file"
				});

				operation.Consumes.Add("multipart/form-data");
			}
		}
	}
}

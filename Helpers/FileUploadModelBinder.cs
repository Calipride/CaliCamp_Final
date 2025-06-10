using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CaliCamp.Helpers
{
    public class FileUploadModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var form = await bindingContext.HttpContext.Request.ReadFormAsync();

            var dto = new Models.Dtos.ImageUploadDto
            {
                File = form.Files.GetFile("File"),
                CampingSpotId = int.Parse(form["CampingSpotId"].ToString()),
                UploadedBy = int.Parse(form["UploadedBy"].ToString()),
                Description = form["Description"].ToString()
            };

            bindingContext.Result = ModelBindingResult.Success(dto);
        }
    }
}

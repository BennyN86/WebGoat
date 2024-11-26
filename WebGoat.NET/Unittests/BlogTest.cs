// using Moq;
// using Xunit;
// using Microsoft.AspNetCore.Mvc.ViewFeatures;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using System.Linq;

// public class BlogPageTests
// {
//     [Fact]
//     public void BlogResponses_ShouldBeProperlyEncoded()
//     {
//         // Arrange
//         var blogEntries = new List<BlogEntry>
//         {
//             new BlogEntry
//             {
//                 Id = 1,
//                 Title = "Test Blog",
//                 Content = "This is a test blog",
//                 Responses = new List<BlogResponse>
//                 {
//                     new BlogResponse { 
//                         Contents = "<script>alert('XSS')</script>", 
//                         Author = "EvilUser", 
//                         ResponseDate = System.DateTime.Now 
//                     }
//                 }
//             }
//         };

//         var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
//         var stringBuilder = new StringBuilder();
//         var writer = new StringWriter(stringBuilder);
//         var viewContext = new ViewContext
//         {
//             ViewData = viewData,
//             Writer = writer,
//         };

//         var htmlHelper = new Mock<IHtmlHelper<IEnumerable<BlogEntry>>>();
//         htmlHelper.Setup(x => x.ViewData).Returns(viewData);

//         // Act
//         var razorView = new BlogPage(); // This would be your Razor page class
//         razorView.Model = blogEntries; // Set the Model
//         razorView.Html = htmlHelper.Object;
//         razorView.ExecuteAsync().Wait();

//         var output = stringBuilder.ToString();

//         // Assert
//         Assert.DoesNotContain("<script>", output); // Ensure no script tags are rendered
//         Assert.Contains("&lt;script&gt;", output); // Ensure the script is encoded
//     }
// }

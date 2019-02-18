using GraphQL.Language.AST;
using GraphQL.Validation;
using SandwichClub.Api.GraphQL;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services;
using Xunit;

namespace SandwichClub.Api.Tests.GraphQL
{
    public class GraphQLAuthenticationValidatorTests : UnitTestBase<GraphQLAuthenticationValidator>
    {
        private IScSession Session => Mock<IScSession>().Object;

        private readonly ValidationContext _context;

        public GraphQLAuthenticationValidatorTests()
        {
            _context = new ValidationContext();

            Mock<IScSession>().SetupAllProperties();
            _context.UserContext = Session;
        }

        [Fact]
        public void TestValidate_WhenNotAuthenticatedAndOnlyAccessingSchema_ShouldNotGenerateErrors()
        {
            // Given
            var selectionSet = new SelectionSet();
            selectionSet.Add(new Field(new NameNode("__schema"), new NameNode("__schema")));
            var operation = new Operation { SelectionSet = selectionSet };

            // When
            var nodeListener = Service.Validate(_context);
            nodeListener.Enter(operation);

            // Verify
            Assert.Empty(_context.Errors);
        }

        [Fact]
        public void TestValidate_WhenNotAuthenticatedAndOnlyAccessingTypes_ShouldNotGenerateErrors()
        {
            // Given
            var selectionSet = new SelectionSet();
            selectionSet.Add(new Field(new NameNode("__types"), new NameNode("__types")));
            var operation = new Operation { SelectionSet = selectionSet };

            // When
            var nodeListener = Service.Validate(_context);
            nodeListener.Enter(operation);

            // Verify
            Assert.Empty(_context.Errors);
        }

        [Fact]
        public void TestValidate_WhenNotAuthenticatedAndAccessingOnlyNonPrivateTypes_ShouldGenerateErrors()
        {
            // Given
            var selectionSet = new SelectionSet();
            selectionSet.Add(new Field(new NameNode("me"), new NameNode("me")));
            var operation = new Operation { SelectionSet = selectionSet };

            // When
            var nodeListener = Service.Validate(_context);
            nodeListener.Enter(operation);

            // Verify
            Assert.NotEmpty(_context.Errors);
        }

        [Fact]
        public void TestValidate_WhenNotAuthenticatedAndOnlyAccessingPrivateAndQueryTypes_ShouldGenerateErrors()
        {
            // Given
            var selectionSet = new SelectionSet();
            selectionSet.Add(new Field(new NameNode("__types"), new NameNode("__types")));
            selectionSet.Add(new Field(new NameNode("me"), new NameNode("me")));
            var operation = new Operation { SelectionSet = selectionSet };

            // When
            var nodeListener = Service.Validate(_context);
            nodeListener.Enter(operation);

            // Verify
            Assert.NotEmpty(_context.Errors);
        }

        [Fact]
        public void TestValidate_WhenAuthenticated_ShouldNotGenerateErrors()
        {
            // Given
            Session.CurrentUser = new User();
            var selectionSet = new SelectionSet();
            selectionSet.Add(new Field(new NameNode("__types"), new NameNode("__types")));
            selectionSet.Add(new Field(new NameNode("me"), new NameNode("me")));
            var operation = new Operation { SelectionSet = selectionSet };

            // When
            var nodeListener = Service.Validate(_context);
            nodeListener.Enter(operation);

            // Verify
            Assert.Empty(_context.Errors);
        }
    }
}

using BGTechTest.Web.API.Validation;
using BGTechTest.WebAPI.Tests.Helpers;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class IdentityNumberValidatorTests
    {
        private IIdentityNumberValidator _identityNumberValidator;
        [SetUp]
        public void Setup()
        {
            _identityNumberValidator = new TestIdentityNumberValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Validate_GivenANullOrEmptyWhiteSpaceIdentityNumber_ReturnsResultWithFalseAndReasons(string identityNumber)
        {
            var result = _identityNumberValidator.Validate(identityNumber);
            Assert.That(result.Isvalid,Is.False);
            Assert.That(result.ErrorMessage,Is.EqualTo("The identity number cannot be null, " +
                                                       "empty or whitespace"));
        }

        [TestCase("8605065AB7083")]
        [TestCase("8605065$%7083")]
        public void Validate_GivenAnIdentityNumberThatHasALetterOrSpecialCharacter_ReturnsResultWithFalseAndReasons
            (string IdentityNumber)
        {
            var result = _identityNumberValidator.Validate(IdentityNumber);
            Assert.That(result.Isvalid,Is.False);
            Assert.That(result.ErrorMessage,Is.EqualTo("The identity number contains " +
                                                       "characters that are not numbers"));
        }

        [TestCase("8605065AB708")]
        [TestCase("8605065$%708")]
        public void
            Validate_GivenAnIdentityNumberThatHasALetterOrSpecialCharacterAndLessThan13InLength_ReturnsResultWithFalseAndReasons
            (string IdentityNumber)
        {
            var result = _identityNumberValidator.Validate(IdentityNumber);
            Assert.That(result.Isvalid,Is.False);
            Assert.That(result.ErrorMessage,Is.EqualTo("The identity number contains " +
                                                       "characters that are not numbers|The Identity number has " +
                                                       "less than 13 digits"));
        }

        [TestCase("8605065AB70831")]
        [TestCase("8605065$%70831")]
        public void
            Validate_GivenAnIdentityNumberThatHasALetterOrSpecialCharacterAndMoreThan13InLength_ReturnsResultWithFalseAndReasons
            (string IdentityNumber)
        {
            var result = _identityNumberValidator.Validate(IdentityNumber);
            Assert.That(result.Isvalid, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("The identity number contains " +
                                                        "characters that are not numbers|The Identity number has " +
                                                        "more than 13 digits"));
        }
        [TestCase("0109046424188")]
        [TestCase("8605065447083")]
        public void Validate_GivenIncorrectIdentityNumber_ReturnsResultWithFalseAndReasons(string identityNumber)
        {
            var result = _identityNumberValidator.Validate(identityNumber);
            Assert.That(result.Isvalid,Is.False);
            Assert.That(result.ErrorMessage,Is.EqualTo($"The invalid control character. Expected: {identityNumber[identityNumber.Length-1]} but got : {_identityNumberValidator.controlNumber}"));
        }
        [TestCase("8709046424188")]
        [TestCase("8605065397083")]
        public void Validate_GivenCorrectIdentityNumber_ReturnsResultWithTrueAndNoReasons(string identityNumber)
        {
            var result = _identityNumberValidator.Validate(identityNumber);
            Assert.That(result.Isvalid, Is.True);
            Assert.That(result.ErrorMessage, Is.EqualTo(""));
        }
    }
}
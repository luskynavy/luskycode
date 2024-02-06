using ConsoleApp;
using TechTalk.SpecFlow;

namespace GherkinUnitTest
{
    [Binding]
    public class CalcSteps
    {
        private int _x;
        private int _y;
        private int _result;

        [Given("that I have 2 integers (.*) and (.*)")]
        public void GivenThatIHave2Integers(int x, int y)
        {
            _x = x;
            _y = y;
        }

        [When("I ask for an addition")]
        public void WhenIAskForAnAddition()
        {
            var c = new Calc();
            _result = c.Add(_x, _y);
        }

        [When("I ask for a multiplication")]
        public void WhenIAskForAMultiplication()
        {
            var c = new Calc();
            _result = c.Multiply(_x, _y);
        }

        [Then("the result is (.*)")]
        public void ThenTheResultIs(int result)
        {
            Assert.AreEqual(result, _result);
        }
    }
}
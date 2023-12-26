using PosBooksCore.ViewModels;

namespace PosBooksTest.ViewModels;

public class ResultViewModelTest
{
    [Fact]
    public void ConstructorWithData_SetsDataProperty()
    {
        var expectedData = "testData";
        var model = new ResultViewModel<string>(expectedData);

        Assert.Equal(expectedData, model.Erros[0].ToString());
        Assert.Null(model.Data);
    }

    [Fact]
    public void ConstructorWithErrors_SetsErrorsProperty()
    {
        var expectedErrors = new List<string>{ "Error1", "Error2" };
        var model = new ResultViewModel<string>(expectedErrors);

        Assert.Null(model.Data);
        Assert.Equal(expectedErrors, model.Erros);
    }
    
    [Fact]
    public void ConstructorWithDataAndErrors_SetsDataAndErrorsProperty()
    {
        var expectedData = "testData";
        var expectedErrors = new List<string> { "Error0", "Error1" };
        var model = new ResultViewModel<string>(expectedData, expectedErrors);

        Assert.Equal(expectedData, model.Data);
        Assert.Equal(expectedErrors, model.Erros);
    }

    [Fact]
    public void ConstructorWithStringError_SetsErrorProperty()
    {
        var expectedError = "Error";
        var model = new ResultViewModel<string>(expectedError);

        Assert.Null(model.Data);
        Assert.Single(model.Erros);
        Assert.Equal(expectedError, model.Erros[0]);
    }
}
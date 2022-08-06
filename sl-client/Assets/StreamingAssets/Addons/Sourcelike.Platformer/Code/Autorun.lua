local TestFunc = {}

function TestFunc.Test(string)
    CLConsole.Log(string)
end

TestFunc.Test = Test

return TestFunc
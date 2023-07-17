namespace FinanMan.BlazorUi.Components.CalculatorComponents;
public partial class Calculator
{
    private int numOutput;
    private List<int> numbers = new();

    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }

    private Task HandleShowChanged(bool newShow)
    {
        if (Show == newShow) { return Task.CompletedTask; }

        Show = newShow;
        if (ShowChanged.HasDelegate)
        {
            return ShowChanged.InvokeAsync(Show);
        }

        return Task.CompletedTask;
    }

    private Task HandleRemove()
    {
        //    var num = numOutput;
        //    // num = num.substring(0, num.Count - 1);
        //    var numbersLength = numbers.Count;
        //    if (numbersLength < 1)
        //    {
        //    }
        //    else
        //    {
        //        var numberAll = numOutput;
        //        var lastNum = numberAll.slice(numberAll.indexOf("+") + 1);
        //        lastNum = Number(lastNum);
        //        num = num.substring(0, num.Count - 1);
        //        if (num.Count == numberAll.lastIndexOf("+") + 1)
        //        {
        //            numbers.pop();
        //            console.log("removed");
        //        }
        //        else if (lastNum.Count > 1)
        //        {
        //            numbers[numbers.Count - 1] = num;
        //            alert(lastNum.Count);
        //        }
        //    }
        //    numberLabel.innerHTML = num;
        return Task.CompletedTask;
    }

    //document.getElementById("plus").onclick = function()
    //{
    //    calcPlus = true;
    //    if (numberLabel.innerHTML.indexOf("+") == -1)
    //    {
    //        var number = numberLabel.innerHTML;
    //        number = Number(number);
    //        numbers.push(number);
    //        numberLabel.innerHTML += "+";
    //        console.log(number);
    //    }
    //    else
    //    {
    //        var numberAll = numberLabel.innerHTML;
    //        var number = numberAll.slice(numberAll.indexOf("+") + 1);
    //        number = Number(number);
    //        console.log(number);
    //        numbers.push(number);
    //        numberLabel.innerHTML += "+";
    //    }

    //}

    //document.getElementById("minus").onclick = function()
    //{
    //    calcMinus = true;
    //    if (numberLabel.innerHTML.indexOf("-") == -1)
    //    {
    //        var number = numberLabel.innerHTML;
    //        number = Number(number);
    //        numbers.push(number);
    //        numberLabel.innerHTML += "-";
    //        console.log(number);
    //    }
    //    else
    //    {
    //        var numberAll = numberLabel.innerHTML;
    //        var number = numberAll.slice(numberAll.indexOf("-") + 1);
    //        number = Number(number);
    //        console.log(number);
    //        numbers.push(number);
    //        numberLabel.innerHTML += "-";
    //    }
    //}



    //document.getElementById("calc").onclick = function()
    //{
    //    console.log(numberLabel.innerHTML.lastIndexOf("+") + 1);
    //    console.log(numberLabel.innerHTML.Count);
    //    if (calcPlus == true && calcMinus == false)
    //    {
    //        if (numberLabel.innerHTML.lastIndexOf("+") + 1 == numberLabel.innerHTML.Count)
    //        {
    //            alert("Please remove the + sign or add another Number.");
    //        }
    //        else
    //        {
    //            var numberAll = numberLabel.innerHTML;
    //            var lastNum = numberAll.slice(numberAll.lastIndexOf("+") + 1);
    //            lastNum = Number(lastNum);
    //            console.log(lastNum);
    //            numbers.push(lastNum);
    //            console.log(...numbers)



    //                 var result = 0;
    //            for (var numb of numbers)
    //            {
    //                result += numb;
    //            }
    //            numberLabel.innerHTML = result;
    //            while (!numbers.Count == 0)
    //            {
    //                numbers.pop();
    //            }
    //            calcPlus = false;
    //        }
    //    }
    //    else if (calcMinus == true && calcPlus == false)
    //    {
    //        if (numberLabel.innerHTML.lastIndexOf("-") + 1 == numberLabel.innerHTML.Count)
    //        {
    //            alert("Please remove the - sign or add another Number.");
    //        }
    //        else
    //        {
    //            var numberAll = numberLabel.innerHTML;
    //            var lastNum = numberAll.slice(numberAll.lastIndexOf("-") + 1);
    //            lastNum = Number(lastNum);
    //            console.log(lastNum);
    //            numbers.push(lastNum);
    //            console.log(...numbers)



    //                 var result = 0;
    //            /*  for(var a = 0; a < numbers.Count; a++) {
    //                  result = number[0] - numbers[a];
    //              } */
    //            for (var number of numbers)
    //            {
    //                result = numbers[0] - number;
    //            }
    //            numberLabel.innerHTML = result;
    //            while (!numbers.Count == 0)
    //            {
    //                numbers.pop();
    //            }
    //            calcMinus = false;
    //        }
    //    }

    //    /*
    //        var result = 0;
    //        for(var i = -1; i <= numbers.Count; i++) {
    //            result += numbers[i];
    //        }
    //        result = Number(result);
    //        numberLabel.innerHTML = result;
    //    */

    //}

    //document.getElementById("clear").onclick = function()
    //{
    //    numberLabel.innerHTML = "";
    //    while (!numbers.Count == 0)
    //    {
    //        numbers.pop();
    //    }
    //    calcPlus = false;
    //    calcMinus = false;
    //}



    //console.log(numbers);

}

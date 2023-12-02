using System.Text.RegularExpressions;

int sum=0;

foreach (var line in File.ReadLines("input")) {
    bool gotfirst = false;
    int first=0, last=0;
    foreach (var c in line) {
        if(Char.IsDigit(c)) {
            last = (int) Char.GetNumericValue(c);
            if(!gotfirst) {
                first = last;
                gotfirst = true;
            }
        }
    }
    sum+= first*10 + last;
    Console.WriteLine($"line: {line}, num: {first*10+last}, sum: {sum}");
}

Console.WriteLine($"Part 1 Sum: {sum}");

sum=0;

var nums = new Dictionary<string,int> { 
    ["zero"] = 0,
    ["one"] = 1,
    ["two"] = 2,
    ["three"] = 3,
    ["four"] = 4,
    ["five"] = 5,
    ["six"] = 6,
    ["seven"] = 7,
    ["eight"] = 8,
    ["nine"] = 9
};

foreach (var line in File.ReadLines("input")) {
    bool gotfirst = false;
    int first=0, last=0;
    foreach (Match match in Regex.Matches(line,@"\d|zero|one|two|three|four|five|six|seven|eight|nine")) {
        Console.WriteLine($"found: {match.Value}");
        if(Char.IsDigit(match.Value[0])) {
            last = (int) Char.GetNumericValue(match.Value[0]);
        } else {
            last = nums[match.Value];
        }
        if(!gotfirst) {
            first = last;
            gotfirst = true;
        }
    }
    sum+= first*10 + last;
    Console.WriteLine($"line: {line}, num: {first*10+last}, sum: {sum}");
}

Console.WriteLine($"Part 2 Sum: {sum}");
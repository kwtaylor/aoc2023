using System.Text.RegularExpressions;

const int OK_RED = 12;
const int OK_GREEN = 13;
const int OK_BLUE = 14;

int sum = 0;
int powersum=0;

foreach (var line in File.ReadLines("input")) {
    var max_draws = new Dictionary<string,int>() {
        ["red"] = 0,
        ["green"] = 0,
        ["blue"] = 0
    };
    Match line_parse = Regex.Match(line,@"Game (\d+): (.+)");

    int game = int.Parse(line_parse.Groups[1].Value);
    string[] turns = Regex.Split(line_parse.Groups[2].Value, ";");

    foreach (var turn in turns) {
        foreach (var draw in Regex.Split(turn, ",")) {
            Match draw_parse = Regex.Match(draw, @"\s*(\d+)\s+(\S+)");
            int num = int.Parse(draw_parse.Groups[1].Value);
            string color = draw_parse.Groups[2].Value;

            if (num > max_draws[color]) 
                max_draws[color] = num;

        }
    }

    Console.WriteLine($"game {game}, red {max_draws["red"]}, green {max_draws["green"]}, blue {max_draws["blue"]}");
    if (max_draws["red"] <= OK_RED &&
        max_draws["green"] <= OK_GREEN &&
        max_draws["blue"] <= OK_BLUE) {
            sum += game;
        }
    
    int power = max_draws["red"]*max_draws["green"]*max_draws["blue"];
    powersum += power;

}

Console.WriteLine(sum);
Console.WriteLine(powersum);
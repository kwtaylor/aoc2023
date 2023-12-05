int sum = 0;

// holds # of winners per card (for part 2)
var card_wins = new List<int>();
// holds list of cards we own (for part 2)
var cards = new Queue<int>();

var card = 0; // starting from zero

foreach (var line in File.ReadAllLines("input")) {
    int matches = 0;
    int points = 0; 
    var winning = new HashSet<int>();

    string[] nums = line.Split(":|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    foreach (var num in nums[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
        winning.Add(int.Parse(num));
        Console.WriteLine($"found: {num}");
    }

    foreach (var num in nums[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
        if(winning.Contains(int.Parse(num))) {
            matches++;
            if(points == 0) points = 1;
            else points *= 2;
            Console.WriteLine($"won: {num} points: {points}");
        }
    }

    card_wins.Add(matches);
    cards.Enqueue(card);
    card++;

    sum += points;
}

Console.WriteLine($"Part 1: {sum}");

// part 2
int cardcount=0;

while(cards.Count > 0) {
    cardcount++;
    int cardn = cards.Dequeue();
    for (int n = cardn+1; n<=cardn+card_wins[cardn]; n++) {
        cards.Enqueue(n);
    }
}

Console.WriteLine($"Part2: {cardcount}");
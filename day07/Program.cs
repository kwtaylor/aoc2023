var hands = new SortedDictionary<string,int>(Comparer<string>.Create(CamelComp));

foreach(var line in File.ReadLines("input")) {
    hands[line[0..5]] = int.Parse(line[6..]);
}

int rank = 1;
int total = 0;

foreach(var kvp in hands) {
    Console.WriteLine(kvp.Key);
    total += kvp.Value*rank;
    rank++;
}

Console.WriteLine(total);


// // CardVal part 1 version
// static int CardVal(char card) => card switch {
//     >= '2' and <= '9' => card-'2',
//     'T' => 8,
//     'J' => 9,
//     'Q' => 10,
//     'K' => 11,
//     'A' => 12,
//     _ => 0
// };

// CardVal part 2 version
static int CardVal(char card) => card switch {
    >= '2' and <= '9' => card-'1',
    'T' => 9,
    'J' => 0,
    'Q' => 10,
    'K' => 11,
    'A' => 12,
    _ => 0
};

static int CamelComp(string x, string y) {
    var x_counts = new int[13];
    var y_counts = new int[13];

    int x_pairs = 0;
    int y_pairs = 0;
    int x_max = 0;
    int y_max = 0;

    foreach (char c in x) {
        var cnt = ++x_counts[CardVal(c)];
        if (c == 'J') continue; // Part 2, deal with jokers later
        if (cnt==2) x_pairs++;
        if (cnt>x_max) x_max = cnt;
    }
    foreach (char c in y) {
        var cnt = ++y_counts[CardVal(c)];
        if (c == 'J') continue; // Part 2, deal with jokers later
        if (cnt==2) y_pairs++;
        if (cnt>y_max) y_max = cnt;
    }

    //Part 2: deal with Jokers
    if(x_counts[0] > 0) {
        x_max += x_counts[0]; // best hand is to increase "max" matching
        if(x_pairs == 0) x_pairs = 1; // Always at least one pair
    }
    if(y_counts[0] > 0) {
        y_max += y_counts[0]; // best hand is to increase "max" matching
        if(y_pairs == 0) y_pairs = 1; // Always at least one pair
    }

    if(x_max < y_max) return -1;
    if(x_max > y_max) return 1;
    if(x_pairs < y_pairs) return -1;
    if(x_pairs > y_pairs) return 1;

    foreach(var xy in x.Zip(y, (x,y) => (x,y))) {
        if(CardVal(xy.x) < CardVal(xy.y)) return -1;
        if(CardVal(xy.x) > CardVal(xy.y)) return 1;
    }
    
    Console.WriteLine($"Tie Seen! {x} {y}");
    return 0;
}

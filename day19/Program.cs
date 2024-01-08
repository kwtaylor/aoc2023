
using System.Numerics;

var flows = new Dictionary<string, List<(char r, char c, int v, string n)>>();
var parts = new List<(int x, int m, int a, int s)>();
bool gotflows = false;

foreach (var line in File.ReadLines("input")) {
    var splits = line.Split(",{}".ToArray(), StringSplitOptions.RemoveEmptyEntries);
    if (!gotflows) {
        if(splits.Length == 0) {
            gotflows=true;
        } else {
            var list = new List<(char r, char c, int v, string n)>();
            foreach (var rule in splits[1..]) {
                var spl = rule.Split(':');
                if(spl.Length > 1) {
                    list.Add((spl[0][0], spl[0][1], int.Parse(spl[0][2..]), spl[1]));
                } else {
                    list.Add(('\0','\0',0,spl[0]));
                }
            }
            flows[splits[0]] = list;
        }
    } else {
        parts.Add(( int.Parse(splits[0][2..]),
                    int.Parse(splits[1][2..]),
                    int.Parse(splits[2][2..]),
                    int.Parse(splits[3][2..])
        ));
    }
}

long total=0;

foreach(var part in parts) {
    var flow = "in";
    while (flow != "R" && flow != "A") {
        foreach (var rule in flows[flow]) {
            if(TestRule(part, rule)) {
                flow = rule.n;
                break;
            }
        }
    }
    if(flow == "A") {
        total+= part.x+part.m+part.a+part.s;
    }
}

Console.WriteLine($"Part 1: {total}");

BigInteger tot_comb = 0;

FindRanges(((1,4000),(1,4000),(1,4000),(1,4000)), "in");

Console.WriteLine($"Part 2: {tot_comb}");


void FindRanges((Range x, Range m, Range a, Range s) range, string flow) {
    Range test_x = range.x;
    Range test_m = range.m;
    Range test_a = range.a;
    Range test_s = range.s;
    foreach(var rule in flows[flow]) {
        Range new_x = test_x;
        Range new_m = test_m;
        Range new_a = test_a;
        Range new_s = test_s;
        var inrange = rule.r switch
        {
            'x' => CheckRange(ref new_x, ref test_x, rule),
            'm' => CheckRange(ref new_m, ref test_m, rule),
            'a' => CheckRange(ref new_a, ref test_a, rule),
            's' => CheckRange(ref new_s, ref test_s, rule),
            _ => true,
        };
        if(inrange) {
            if (rule.n == "A") {
                tot_comb += Combos((new_x,new_m,new_a,new_s));
            } else if (rule.n != "R") {
                FindRanges((new_x,new_m,new_a,new_s), rule.n);
            }
        }
    }
}

bool CheckRange(ref Range r, ref Range r_opp, (char r, char c, int v, string n) rule) {
    switch (rule.c) {
        case '>':
            if(r.max > rule.v) {
                r.min = rule.v+1;
                r_opp.max = rule.v;
                return true;
            }
            break;
        case '<':
            if(r.min < rule.v) {
                r.max = rule.v-1;
                r_opp.min = rule.v;
                return true;
            }
            break;
        default:
            return true;
    }
    return false;
}

BigInteger Combos((Range x, Range m, Range a, Range s) range) {
    return (BigInteger)(range.x.max-range.x.min+1)*
           (BigInteger)(range.m.max-range.m.min+1)*
           (BigInteger)(range.a.max-range.a.min+1)*
           (BigInteger)(range.s.max-range.s.min+1);
}

bool TestRule((int x, int m, int a, int s) part, (char r, char c, int v, string n) rule) {
    int val = 0;
    switch (rule.r) {
        case 'x': 
            val = part.x;
            break;
        case 'm': 
            val = part.m;
            break;
        case 'a': 
            val = part.a;
            break;
        case 's': 
            val = part.s;
            break;
        default: 
            return true;
    }
    if(rule.c == '>' && val > rule.v) return true;
    if(rule.c == '<' && val < rule.v) return true;

    return false;
}


struct Range {
    public static implicit operator Range((int min, int max) r) {
        return new Range() {min=r.min, max=r.max};
    }

    public override string ToString () {
        return $"Range({min},{max})";
    }
    public int min;
    public int max;
};
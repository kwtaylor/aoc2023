using System.Numerics;

var input = new StreamReader("input");

string dir = input.ReadLine();
input.ReadLine(); // blank line

var map = new Dictionary<string,(string l,string r)>();

string line;
while( (line = input.ReadLine()) != null) {
    string[] p = line.Split(" =(),".ToArray(), StringSplitOptions.RemoveEmptyEntries);
    map[p[0]] = (p[1],p[2]);
}

string pos = "AAA";
int cnt = 0;
int idx = 0;
int len = dir.Length;

Console.WriteLine($"Length {len} Nodes {map.Count}");

while(pos != "ZZZ") {
    if(idx==len) idx = 0;
    var m = map[pos];
    pos = dir[idx] switch {
        'L' => m.l,
        'R' => m.r
    };
    cnt++;
    idx++;
}

Console.WriteLine($"Part 1: {cnt}");

var ghosts = new List<string>();

foreach (var m in map) {
    if(m.Key[2] == 'A') {
        ghosts.Add(m.Key);
    }
}

// hack: input is structured so length from start->end equals length from end->end
//       and every start only has one end
//       so just measure the first length and find LCMs
var ghost_len = new List<long>();

foreach (var g in ghosts) {
    pos = g;
    idx = 0;
    cnt = 0;
    while(pos[2] != 'Z') {
        if(idx==len) idx = 0;
        var m = map[pos];
        pos = dir[idx] switch {
            'L' => m.l,
            'R' => m.r
        };
        cnt++;
        idx++;
    }
    Console.WriteLine($"Ghost {g} Len {cnt} End {pos}");
    ghost_len.Add(cnt);
}

// BigInteger has a GCD function but there's no generic one? whatev, it works with casts
long gcf = ghost_len.Aggregate(ghost_len[0],(l,r) => (long) BigInteger.GreatestCommonDivisor(l,r));
var facts = ghost_len.Select(x => x/gcf);
Console.WriteLine($"GCF {gcf} Facts {String.Join(' ',facts)}");
long mult = gcf * facts.Aggregate(1L, (l,r) => l*r);

Console.WriteLine($"Part 2: {mult}");
using System.Text;

Console.WriteLine("Enter the path:");

string? path = Console.ReadLine();

if (path == null) {
    Console.WriteLine("Path is null");
    return;
}

var file = File.ReadAllLines(path);

if(file == null) {
    Console.WriteLine("File is null");
    return;
}

Console.WriteLine("Enter output path:");
string? outPath = Console.ReadLine();

if (outPath == null) {
    Console.WriteLine("Output path is null");
    return;
}

BinaryWriter fWriter = new BinaryWriter(File.Open(outPath, FileMode.Create));

fWriter.Write((byte)(BitConverter.IsLittleEndian ? 1 : 0));

List<string> palletes = new List<string>();

float parseIEEE754(string s) {
    // Convert the hexadecimal string to bytes
    byte[] bytes = new byte[4];
    for (int i = 0; i < 4; i++) {
        bytes[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
    }

    // Reverse bytes if the system is little-endian
    if (BitConverter.IsLittleEndian) {
        Array.Reverse(bytes);
    }

    // Convert the byte array back into a float
    return BitConverter.ToSingle(bytes, 0);
}

MemoryStream mes = new MemoryStream();
BinaryWriter writer = new BinaryWriter(mes);

for (int i = 0; i < file.Length; i++) {
    string command = file[i].Split(" ")[0];
    string[] ps = file[i].Substring(command.Length + 1).Split(";");
    if (command == "LINE") {
        int count = int.Parse(ps[0]);
        string color = ps[1];
        float width = float.Parse(ps[2]);

        int colorIndex;
        if (!palletes.Contains(color)) {
            palletes.Add(color);
            colorIndex = palletes.Count - 1;
        } else {
            colorIndex = palletes.IndexOf(color);
        }

        writer.Write((byte)1);
        writer.Write((byte)colorIndex);
        writer.Write((float)width);
        writer.Write(count);
        for (int j = 0; j < count; j++) {
            i += 1;
            string[] point = file[i].Split(" ");
            writer.Write(parseIEEE754(point[0]));
            writer.Write(parseIEEE754(point[1]));
        }
    } else if (command == "FILL") {
        int count = int.Parse(ps[0]);
        string color = ps[1];

        int colorIndex;
        if (!palletes.Contains(color)) {
            palletes.Add(color);
            colorIndex = palletes.Count - 1;
        } else {
            colorIndex = palletes.IndexOf(color);
        }

        writer.Write((byte)2);
        writer.Write((byte)colorIndex);
        writer.Write(count);
        for (int j = 0; j < count; j++) {
            i += 1;
            string[] point = file[i].Split(" ");
            writer.Write(parseIEEE754(point[0]));
            writer.Write(parseIEEE754(point[1]));
        }
    } else if (command == "CURVE") {
        int count = int.Parse(ps[0]);
        string color = ps[1];
        float width = float.Parse(ps[2]);

        int colorIndex;
        if (!palletes.Contains(color)) {
            palletes.Add(color);
            colorIndex = palletes.Count - 1;
        } else {
            colorIndex = palletes.IndexOf(color);
        }

        writer.Write((byte)3);
        writer.Write((byte)colorIndex);
        writer.Write((float)width);
        writer.Write(count);
        for (int j = 0; j < count; j++) {
            i += 1;
            string[] point = file[i].Split(" ");
            writer.Write(parseIEEE754(point[0]));
            writer.Write(parseIEEE754(point[1]));
        }
    } else if (command == "BEZ") {
        int count = int.Parse(ps[0]);
        string color = ps[1];
        float width = float.Parse(ps[2]);

        int colorIndex;
        if (!palletes.Contains(color)) {
            palletes.Add(color);
            colorIndex = palletes.Count - 1;
        } else {
            colorIndex = palletes.IndexOf(color);
        }

        writer.Write((byte)4);
        writer.Write((byte)colorIndex);
        writer.Write((float)width);
        writer.Write(count);
        for (int j = 0; j < count; j++) {
            i += 1;
            string[] point = file[i].Split(" ");
            writer.Write(parseIEEE754(point[0]));
            writer.Write(parseIEEE754(point[1]));
        }
    } else if(command == "TEXT") {
        writer.Write((byte)5);
        string text = file[i].Substring(command.Length + 1);
        var bytes = Encoding.UTF8.GetBytes(text);
        writer.Write(bytes.Length);
        writer.Write(bytes);

        i += 1;
        string[] point = file[i].Split(" ");
        writer.Write(parseIEEE754(point[0]));
        writer.Write(parseIEEE754(point[1]));
    } else {
        Console.WriteLine($"Unrecognized command: {command}");
    }
}
writer.Write((byte)15); // End of commands
fWriter.Write((byte)palletes.Count);
foreach (string color in palletes) {
    var bytes = Encoding.UTF8.GetBytes(color);
    fWriter.Write(bytes.Length);
    fWriter.Write(bytes);
}

fWriter.Write(mes.ToArray());

writer.Close();
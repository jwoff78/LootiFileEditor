# LootiFileEditor
Looti file Editor - Version official 1.0
<br><br>
Syntax:
```
Looti.Looti.Run(string PATH, string[] argv);
```
<br>
Directly to Looti:

```
Looti.Looti.Editor(string PATH, string[] argv);
```
<br>
<br>

## Arguments

Looti can be runned with few arguments which must be specified  into `string[] argv`.

### Arguments:

`--help` - help<br>
`--mono` - monochromatic mode<br>
`--color` - color mode<br>
`--lock` - only to read mode<br>
`--ide` - Run Looti in IDE mode<br>

## Configuration file

Looti automatically creating `Looti.scf` with editor configuration. If configuration in `looti.scf` conflict with information in a variable
`string[] argv`, the information in argv is finally used. <br>

### Configuration:

`colors=yes/no` - use colors<br>
`beep=yes/no` - beep on start<br>
`hello=yes/no` - show splash screen<br>
`click=yes/no` - make sound on click<br>
`wraplines=yes/no` - wrap lines<br>
`IDE=yes/no` - use Looti in IDE mode<br>
`cursor= ` - use " " (space) as cursor. If cursor char lenght is longer than one char, Looti uses " " (space) as cursor.

<br>Settings `IDE` and `click` are default `false`



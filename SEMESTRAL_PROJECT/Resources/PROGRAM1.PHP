<?php
// Program which prints numbers from one to defined one
string $input = fgets(STDIN);
int $counter = intval($input);
int $i = 1;
while(1 == 1)
{
    echo(strval($i));
    $i++;
    if ($i > $counter)
    {
        break;
    }
}
?>

<?php
function exp(float $base, int $exponent)
{
    float $reti = $base;
    int $i = 1;
    while ($i < $exponent)
    {
        $reti = $reti * $base;
        $i++;
    }
    return $reti;
}
echo(strval(exp(2,3)));
echo(strval(exp(1,5)));
?>

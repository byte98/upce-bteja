<?php
// Program which computes power of real number

// Function which computes power of real number
function exp(float $base, int $exponent)
{
    float $reti = $base;
    int $i = 0;
    while ($i < $exponent)
    {
        $reti = $reti * $base;
        $i++;
    }
    return $reti;
}
echo("Enter base:");
string $input = fgets(STDIN);
float $b = floatval($input);
while ($b != 0)
{
    echo("Enter exponent:");
    $input = fgets(STDIN);
    int $e = intval($input);
    float $result = 0;
    $result = exp($b, $e);
    echo ($reti);
    echo("Enter base:");
    $input = fgets(STDIN);
    $b = floatval($input);
}
?>

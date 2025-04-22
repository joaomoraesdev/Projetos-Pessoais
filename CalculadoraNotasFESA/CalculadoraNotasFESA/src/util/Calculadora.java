package util;

import java.util.Locale;
import java.util.Scanner;

public class Calculadora {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in).useLocale(Locale.US);
        double n1b1 = Double.valueOf(args[0]);
        double n2b1 = Double.valueOf(args[1]);
        double n1b2 = Double.valueOf(args[2]);
        double n2b2 = Double.valueOf(args[3]);

        // Primeiro Bimestre
        double media1 = (n1b1 * 0.4) + (n2b1 * 0.6);
        // Segundo Bimestre
        double media2 = (n1b1 * 0.4) + (n2b1 * 0.6);
        // Nota Semestre
        double mediaFinal = (media1 * 0.5) + (media2 * 0.5);
        System.out.println(mediaFinal);
    }
}
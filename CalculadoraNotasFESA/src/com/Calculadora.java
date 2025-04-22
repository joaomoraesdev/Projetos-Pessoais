package com;
import java.util.Locale;
import java.util.Scanner;

public class Calculadora {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in).useLocale(Locale.US);

        System.out.println("=== CALCULADORA DE NOTAS FESA ===");

        System.out.print("Matéria: ");
        String materia = scanner.nextLine().toUpperCase();

        System.out.println("");
        System.out.println("Primeiro Bimestre");
        System.out.print("Nota N1B1: ");
        double n1b1 = scanner.nextDouble();

        System.out.print("Nota N2B1: ");
        double n2b1 = scanner.nextDouble();

        System.out.println("");
        System.out.println("Segundo Bimestre");
        System.out.print("Nota N1B2: ");
        double n1b2 = scanner.nextDouble();

        System.out.print("Nota Formativa: ");
        double formativa = scanner.nextDouble();

        System.out.print("Nota N2B2: ");
        double n2b2 = scanner.nextDouble();

        // Primeiro Bimestre
        double media1 = (n1b1 * 0.4) + (n2b1 * 0.6);
        // Segundo Bimestre
        double media2 = ((n1b2 * 0.5 + formativa * 0.5) * 0.4) + (n2b2 * 0.6);
        // Nota Semestre
        double mediaFinal = (media1 * 0.5) + (media2 * 0.5);
        
        System.out.println("");
        System.out.println(String.format("=== %s ===", materia));
        System.out.println(String.format("Nota 1°Bi: %s", media1));
        System.out.println(String.format("Nota 2°Bi: %s", media2));
        System.out.println(String.format("Nota Final: %s", mediaFinal));
    }
}
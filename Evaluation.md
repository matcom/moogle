# Guía para la evaluación

La evaluación consta de tres partes:

- la implementación
- el informe escrito
- la exposición oral

Cada una de estas partes aporta a la evaluación final en igual grado, y todas deben ser evaluadas de forma satisfactoria para que el proyecto se considere aprobado.

Al momento de la entrega se deben haber entregado el código fuente y el informe escrito. Durante la evaluación, el estudiante hará una presentación oral de un máximo de 15 minutos donde expondrá las ideas fundamentales del proyecto. Luego, el tribunal podrá hacer preguntas para evacuar las dudas que queden.

En el momento de la exposición, el estudiante podrá presentar mejoras a las funcionalidades y/o al informe escrito, que serán tenidas en cuenta para la evaluación final, siempre que haya entregado un proyecto funcional en el momento de la entrega final.

A continuación se presenta una guía de evaluación con una serie de criterios concretos que serán tenidos en cuenta, divididos en cuatro partes, tres obligatorias y una opcional. Cada criterio obligatorio será evaluado de *(I)nsuficiente*, *(S)atisfactorio*, o *(E)xcelente*. Para considerar el proyecto aprobado, todos los criterios obligatorios deben ser cumplidos al menos de forma satisfactoria. En caso contrario, el estudiante deberá solventar las deficiencias indicadas para la evaluación extraordinaria. Además, se evaluarán un conjunto extra de criterios opcionales extra tenidos en cuenta como bonificación para la nota final.

## 1. Implementación

El proyecto será evaluado en términos de las funcionalidades implementadas, teniendo en cuenta además los criterios básicos de extensibilidad, mantenibilidad y buenas prácticas.

Criterios a tener en cuenta sobre las funcionalidades implementadas:

- 1.a) Representación implícita o explícita de los documentos y consultas como conjuntos de palabras que permita identificar eficientemente si un término aparece en un documento (_eficientemente_ significa con un costo sublineal).

- 1.b) Implementación de un mecanismo para computar un valor de ranking dados un documento y una consulta.

- 1.c) Diseño de una función de ranking que represente alguna noción de relevancia sensata (e.j., TF-IDF).

- 1.d) Implementación de un conjunto de operadores adicionales que modifican el criterio de relevancia.

- 1.e) Implementación de un mecanismo para computar un fragmento (_snippet_) de cada documento que corresponda con algún criterio de relevancia sensato.

- 1.f) Implementación de un mecanismo para computar sugerencias ante términos para los que no se obtienen resultados.

Además de las funcionalidades, se tendrá en cuenta la mantenibilidad y legibilidad del código:

- 1.g) Organización del código de forma modular, con clases y métodos independientes para las funcionalidades principales.

- 1.h) Documentación (comentarios) a nivel de código para las partes más complejas del proyecto.

## 2. Informe escrito

El informe escrito debe describir la solución presentada en suficiente detalle que permita evaluar su correctitud. Por este motivo no debe solamente mencionar las funcionalidades implementadas, sino además explicar en detalle la representación de los documentos y consultas así como cada algoritmo implementado que no sea trivial (e.j., la búsqueda secuencial en un *array* es trivial).

Criterios a tener en cuenta sobre el contenido:

- 2.a) Descripción de la arquitectura básica del proyecto y el flujo de los datos durante la ejecución de la búsqueda.

- 2.b) Descripción de las funcionalidades implementadas en términos de su uso, e.j., los operadores existentes.

- 2.c) Descripción de la modelación empleada para representar los documentos y consultas, y para computar una función de ranking.

- 2.d) Descripción del funcionamiento de los algoritmos no triviales implementados.

Además del contenido, se tendrán en cuenta criterios sobre la redacción del documento.

- 2.e) Correctitud ortográfica y gramatical.

- 2.f) Claridad y legibilidad en las explicaciones.

## 3. Exposición oral

La exposición oral será un ejercicio de una duración máxima de 15 minutos donde el estudiante expondrá las ideas básicas del proyecto y los detalles más interesantes sobre la implementación. No se debe intentar exponer todos los detalles del proyecto, sino las ideas más importantes.

Los criterios a evaluar son:

- 3.a) Presentación de las ideas más importantes del proyecto: la arquitectura del sistema, la representación de los documentos y consultas, y el criterio de ranking.

- 3.b) Presentación de los detalles de implementación más notables del proyecto.

Además del contenido, se evaluará también la calidad de la presentación y el dominio del tema demostrado por el estudiante:

- 3.c) Calidad general de la presentación, incluyendo fluidez, claridad en las ideas, y buena dicción.

- 3.d) Respuesta a las preguntas formuladas demostrando dominio del proyecto y comprensión profunda de las ideas fundamentales del proyecto y los detalles técnicos.

## 4. Extras

A continuación se presentan algunos criterios opcionales que serán evaluados como _(N/A) no aplica_ o _(A)plica_, y serán tenidos en cuenta

- 4.a) Implementación eficiente del cómputo de ranking a partir de operaciones matriciales.

- 4.b) Implementación de operadores adicionales no descritos en la orientación.

- 4.c) Implementación de transformaciones sintácticas o morfológicas (e.j., _stemming_ o lematización).

- 4.d) Mecanismos de caché para evitar cargar los documentos en cada ejecución.

Cualquier otra adición a las funcionalidades, ya sea en la representación, los algoritmos implementados, o la interfaz gráfica, será anotada por el tribunal y considerada como posible bonificación a la evaluación.

Además, el tribunal documentará cualquier comentario o nota de interés sobre el proyecto que pueda ser tenida en cuenta para la evaluación final.

## Evaluación final

El tribunal otorgará una evaluación general de *(I)nsuficiente*, *(S)atisfactorio*, o *(E)xcelente* para cada una de las tres partes de la evaluación, la implementación, el informe escrito y la exposición. Esta evaluación no será decidida de immediato, sino posteriormente a las exposiciones de todos los estudiantes.

Se considerará *insuficiente* si existe algún criterio que no haya sido evaluado al menos de *satisfactorio*. Se considerará *excelente* si una abrumadora mayoría de los criterios son evaluados de *excelente* (no existe un porciento específico de criterios, queda a decisión del tribunal). En cualquier otro caso, la evaluación será *satisfactoria*.

La evaluación final del proyecto será *satisfactoria* si y solo si **las tres partes** son evaluadas al menos de forma satisfactoria, y será *excelente* si y solo si **las tres partes** son evaluadas de excelente.

La nota final de la asignatura será una combinación entre esta evaluación y los resultados obtenidos en los exámenes parciales. Los criterios extra pueden ser utilizados para compensar a aquellos estudiantes que no cumplan los requisitos mínimos de los exámenes parciales, o para otorgar una nota final de 5 puntos a aquellos estudiantes que hayan cumplido con todos los requisitos mínimos.

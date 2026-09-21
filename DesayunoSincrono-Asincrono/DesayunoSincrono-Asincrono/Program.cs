// Práctica 7: Desayuno Asíncrono
// "No se trata de correr más rápido, sino de saber qué carreras correr en paralelo."

using System.Diagnostics;

const int limiteMs = 500; // tiempo límite antes de que el café se enfríe

// ==========================================
// SOLUCIONES SIN TIMEOUT
// ==========================================

Console.WriteLine("=== 1. SÍNCRONO (SECUENCIAL) ===");
var t1 = Stopwatch.StartNew();
HacerDesayunoSincrono(CancellationToken.None);
t1.Stop();
Console.WriteLine($"⏱️ Tiempo síncrono: {t1.ElapsedMilliseconds} ms\n");

Console.WriteLine("=== 2. ASYNC/AWAIT (SECUENCIAL) ===");
var t2 = Stopwatch.StartNew();
await HacerDesayunoSecuencialAsync(CancellationToken.None);
t2.Stop();
Console.WriteLine($"⏱️ Tiempo async/await secuencial: {t2.ElapsedMilliseconds} ms\n");

Console.WriteLine("=== 3. ASYNC/AWAIT PARALELO ===");
var t3 = Stopwatch.StartNew();
await HacerDesayunoParaleloAsync(CancellationToken.None);
t3.Stop();
Console.WriteLine($"⏱️ Tiempo async/await paralelo: {t3.ElapsedMilliseconds} ms\n");

Console.WriteLine("=== 4. ASYNC/AWAIT PARALELO OPTIMIZADO ===");
var t4 = Stopwatch.StartNew();
await HacerDesayunoParaleloOptimizadoAsync(CancellationToken.None);
t4.Stop();
Console.WriteLine($"⏱️ Tiempo async/await optimizado: {t4.ElapsedMilliseconds} ms\n");

// ==========================================
// SOLUCIONES CON TIMEOUT DE 500ms
// ==========================================
Console.WriteLine("=== 5. SOLUCIONES CON TIMEOUT DE 500 ms ===");
await EjecutarConTimeout(token => Task.Run(() => HacerDesayunoSincrono(token)), limiteMs, "Síncrono");
await EjecutarConTimeout(HacerDesayunoSecuencialAsync, limiteMs, "Async/await secuencial");
await EjecutarConTimeout(HacerDesayunoParaleloAsync, limiteMs, "Async/await paralelo");
await EjecutarConTimeout(HacerDesayunoParaleloOptimizadoAsync, limiteMs, "Async/await optimizado");

Console.WriteLine("\nFin del programa.");

// ==========================================
// MÉTODO AUXILIAR PARA APLICAR TIMEOUT
// ==========================================
async Task EjecutarConTimeout(Func<CancellationToken, Task> accionDesayuno, int timeoutMs, string nombreSolucion) {
    var timer = Stopwatch.StartNew();
    using var cts = new CancellationTokenSource(timeoutMs);

    try {
        await accionDesayuno(cts.Token);
        timer.Stop();
        Console.WriteLine($"✅ [{nombreSolucion}] ¡Desayuno completado a tiempo en {timer.ElapsedMilliseconds} ms!");
    }
    catch (OperationCanceledException) {
        timer.Stop();
        Console.WriteLine($"❌ [{nombreSolucion}] Cancelado a los {timer.ElapsedMilliseconds} ms.");
        Console.WriteLine("   ☕ ¡El café se ha enfriado! Los huevos y tostadas con café frío no tienen gracia...");
    }
}

// ==========================================
// MÉTODOS Y FUNCIONES
// ==========================================

// --- ACCIONES SÍNCRONAS ---
void Esperar(int ms, CancellationToken token) {
    token.ThrowIfCancellationRequested();
    Thread.Sleep(ms);
    token.ThrowIfCancellationRequested();
}

void PrepararCafe(CancellationToken token) {
    Console.WriteLine("Preparando el cafe...");
    Esperar(200, token);
    Console.WriteLine("Cafe listo");
}

void CalentarSarten(CancellationToken token) {
    Console.WriteLine("Calentando sarten....");
    Esperar(200, token);
    Console.WriteLine("Sarten lista");
}

void FreirHuevos(CancellationToken token) {
    Console.WriteLine("Friendo lo huevoss....ohhhh");
    Esperar(300, token);
    Console.WriteLine("Huevos listos");
}

void FreirBacon(CancellationToken token) {
    Console.WriteLine("Friendo bacon.....");
    Esperar(300, token);
    Console.WriteLine("Bacon listo");
}

void TostarPan(CancellationToken token) {
    Console.WriteLine("Tostando el pan...");
    Esperar(200, token);
    Console.WriteLine("Pan listo");
}

void UntarPan(CancellationToken token) {
    Console.WriteLine("Untando el pan....");
    Esperar(100, token);
    Console.WriteLine("Pan untado y listo");
}

void PrepararZumo(CancellationToken token) {
    Console.WriteLine("Preoarando el zumo");
    Esperar(200, token);
    Console.WriteLine("Zumo preparado");
}

void HacerDesayunoSincrono(CancellationToken token) {
    PrepararCafe(token);
    CalentarSarten(token);
    FreirHuevos(token);
    FreirBacon(token);
    TostarPan(token);
    UntarPan(token);
    PrepararZumo(token);
}

// --- ACCIONES ASÍNCRONAS ---
async Task PrepararCafeAsync(CancellationToken token) {
    Console.WriteLine("Preparando el cafe...");
    await Task.Delay(200, token);
    Console.WriteLine("Cafe preparado");
}

async Task CalentarSartenAsync(CancellationToken token) {
    Console.WriteLine("Calentando sarten....");
    await Task.Delay(200, token);
    Console.WriteLine("Sarten preparada");
}

async Task FreirHuevosAsync(CancellationToken token) {
    Console.WriteLine("Friendo los huevos....");
    await Task.Delay(300, token);
    Console.WriteLine("Huevos preparados");
}

async Task FreirBaconAsync(CancellationToken token) {
    Console.WriteLine("Friendo bacon.....");
    await Task.Delay(300, token);
    Console.WriteLine("Bacon preparado");
}

async Task TostarPanAsync(CancellationToken token) {
    Console.WriteLine("Tostando el pan...");
    await Task.Delay(200, token);
    Console.WriteLine("Pan preparado");
}

async Task UntarPanAsync(CancellationToken token) {
    Console.WriteLine("Untando el pan....");
    await Task.Delay(100, token);
    Console.WriteLine("Pan untado y preparado");
}

async Task PrepararZumoAsync(CancellationToken token) {
    Console.WriteLine("Preparando el zumo...");
    await Task.Delay(200, token);
    Console.WriteLine("Zumo preparado");
}

async Task PrepararPanAsync(CancellationToken token) {
    await TostarPanAsync(token);
    await UntarPanAsync(token);
}

// --- DESAYUNOS COMPLETOS ---
async Task HacerDesayunoSecuencialAsync(CancellationToken token) {
    await PrepararCafeAsync(token);
    await CalentarSartenAsync(token);
    await FreirHuevosAsync(token);
    await FreirBaconAsync(token);
    await TostarPanAsync(token);
    await UntarPanAsync(token);
    await PrepararZumoAsync(token);
}

async Task HacerDesayunoParaleloAsync(CancellationToken token) {
    Task cafeTask = PrepararCafeAsync(token);
    Task zumoTask = PrepararZumoAsync(token);
    Task sartenTask = CalentarSartenAsync(token);
    Task panTask = PrepararPanAsync(token);

    await sartenTask;

    Task huevosTask = FreirHuevosAsync(token);
    Task baconTask = FreirBaconAsync(token);

    await Task.WhenAll(cafeTask, zumoTask, panTask, huevosTask, baconTask);

    Console.WriteLine("¡Desayuno listo sin errores!");
}

async Task HacerDesayunoParaleloOptimizadoAsync(CancellationToken token)
{
    // 1. Cadena 1: Café (200ms) -> Zumo (200ms) = Total 400ms
    Task cafeYZumoTask = PrepararCafeAsync(token)
        .ContinueWith(async _ => await PrepararZumoAsync(token), token).Unwrap();

    // 2. Cadena 2: Tostar Pan (200ms) -> Untar Mantequilla (100ms) = Total 300ms
    Task panYMantequillaTask = PrepararPanAsync(token); // PrepararPanAsync ya incluye Tostar + Untar

    // 3. Cadena 3: Calentar sartén (200ms) -> Huevos (300ms) y Bacon (300ms) en paralelo = Total 500ms
    Task cocinarSartenTask = Task.Run(async () =>
    {
        await CalentarSartenAsync(token);

        Task huevosTask = FreirHuevosAsync(token);
        Task baconTask = FreirBaconAsync(token);

        await Task.WhenAll(huevosTask, baconTask);
    }, token);

    // 4. Se ejecutan las 3 cadenas en paralelo y se espera a la más lenta (la sartén: 500ms)
    await Task.WhenAll(cafeYZumoTask, panYMantequillaTask, cocinarSartenTask);
    
    Console.WriteLine("¡Desayuno optimizado completado!");
}
import { test, expect } from '@playwright/test';

test('tournament list page should load', async ({ page }) => {
  await page.goto('http://localhost:4200');
  await expect(page).toHaveTitle(/AdminConsole/);
});

test('tournament list page should show a title', async ({ page }) => {
  await page.goto('http://localhost:4200');
  const title = page.locator('h1');
  await expect(title).toHaveText('Welcome to the Admin Console');
});

test('tournament list page should show a table with tournaments', async ({ page }) => {
  await page.goto('http://localhost:4200');
  const table = page.locator('table');
  await expect(table).toBeVisible();
});

test('tournament list should have the correct columns', async ({ page }) => {
  await page.goto('http://localhost:4200');
          const headers = page.locator('table thead th');
          await expect(headers).toHaveCount(9);
          await expect(headers).toHaveText(['Tournament', 'Location', 'Level', 'Surface',
            'Men\'s Singles', 'Women\'s Singles', 'Men\'s Doubles', 'Women\'s Doubles', 'Mixed Doubles' ]);
});

test('tournament list should show the eight active tournaments', async ({ page }) => {
  await page.goto('http://localhost:4200');
  const rows = page.locator('table tbody tr');
  await expect(rows).toHaveCount(8);
});
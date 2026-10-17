import { expect } from '@playwright/test';
import { createBdd, DataTable } from 'playwright-bdd';

const { Given, When, Then } = createBdd();

Given('I access the tour management page', async ({ page }) => {
  await page.goto('http://localhost:4200');
  await expect(page).toHaveTitle(/AdminConsole/);
});

Then('I should see a list of the active tournaments', async ({ page }) => {
  const title = page.locator('h1');
  await expect(title).toHaveText('Welcome to the Admin Console');
  const table = page.locator('table');
  await expect(table).toBeVisible();
});

Then('The list should contain the correct set of tournaments', async ({ page }) => {
  const rows = page.locator('table tbody tr');
  await expect(rows).toHaveCount(8);
});

Then('The properties of each tournament are displayed', async ({ page }, dataTable: DataTable) => {
  const headers = page.locator('table thead th');
  const expectedHeaders = dataTable.raw().map(([header]) => header);
  await expect(headers).toHaveCount(expectedHeaders.length);
  await expect(headers).toHaveText(expectedHeaders);
});
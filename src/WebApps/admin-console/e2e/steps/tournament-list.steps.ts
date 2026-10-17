import { expect, Page } from '@playwright/test';
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
  await expect(rows).toHaveCount(ACTIVE_TOURNAMENT_COUNT);
});

const ACTIVE_TOURNAMENT_COUNT = 8;

const showInactiveOption = (page: Page) =>
  page.getByRole('switch', { name: 'Show inactive tournaments' });

Then('The option for showing inactive tournaments is {string}', async ({ page }, state: string) => {
  if (state !== 'on' && state !== 'off') {
    throw new Error(`Unsupported inactive tournaments option state: ${state}`);
  }

  await expect(showInactiveOption(page)).toBeChecked({ checked: state === 'on' });
});

When('I opt to {string} inactive tournaments', async ({ page }, action: string) => {
  if (action !== 'view' && action !== 'hide') {
    throw new Error(`Unsupported inactive tournaments action: ${action}`);
  }

  await showInactiveOption(page).setChecked(action === 'view');
});

Given('I have inactive tournaments shown', async ({ page }) => {
  await showInactiveOption(page).check();
  await expect(showInactiveOption(page)).toBeChecked();
});

Then('Inactive tournaments are included in the list', async ({ page }) => {
  const rows = page.locator('table tbody tr');
  await expect
    .poll(async () => rows.count())
    .toBeGreaterThan(ACTIVE_TOURNAMENT_COUNT);
});

Then('Inactive tournaments are not shown', async ({ page }) => {
  const rows = page.locator('table tbody tr');
  await expect(rows).toHaveCount(ACTIVE_TOURNAMENT_COUNT);
});

Then('The properties of each tournament are displayed', async ({ page }, dataTable: DataTable) => {
  const headers = page.locator('table thead th');
  const expectedHeaders = dataTable.raw().map(([header]) => header);
  await expect(headers).toHaveCount(expectedHeaders.length);
  await expect(headers).toHaveText(expectedHeaders);
});
